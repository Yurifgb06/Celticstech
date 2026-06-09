using Celticstech.Data;
using Celticstech.DTOs;
using Celticstech.Models;
using Celticstech.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Celticstech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly OpenMeteoService _openMeteoService;

        public DiagnosticoController(AppDbContext context, OpenMeteoService openMeteoService)
        {
            _context = context;
            _openMeteoService = openMeteoService;
        }

        [HttpGet("regiao/{idRegiao:int}/cultivo/{idCultivo:int}")]
        public async Task<ActionResult<DiagnosticoResponseDTO>> GetDiagnosticoPorRegiaoECultivo(
            int idRegiao,
            int idCultivo,
            int? idAssociacao,
            CancellationToken cancellationToken)
        {
            return await GerarDiagnostico(idRegiao, idCultivo, idAssociacao, cancellationToken);
        }

        [HttpGet("regiao/{idRegiao:int}")]
        public async Task<ActionResult<DiagnosticoResponseDTO>> GetDiagnostico(
            int idRegiao,
            int? idAssociacao,
            int? idCultivo,
            CancellationToken cancellationToken)
        {
            return await GerarDiagnostico(idRegiao, idCultivo, idAssociacao, cancellationToken);
        }

        private async Task<ActionResult<DiagnosticoResponseDTO>> GerarDiagnostico(
            int idRegiao,
            int? idCultivo,
            int? idAssociacao,
            CancellationToken cancellationToken)
        {
            var regiao = await _context.Regioes
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.IdRegiao == idRegiao, cancellationToken);

            if (regiao == null)
                return NotFound("Regiao nao encontrada.");

            var cultivo = idCultivo.HasValue
                ? await _context.Cultivos.AsNoTracking().FirstOrDefaultAsync(c => c.IdCultivo == idCultivo.Value, cancellationToken)
                : await _context.Cultivos.AsNoTracking().OrderBy(c => c.IdCultivo).FirstOrDefaultAsync(cancellationToken);

            if (cultivo == null)
                return NotFound("Cultivo nao encontrado. Cadastre ou selecione um cultivo valido.");

            try
            {
                var clima = await _openMeteoService.ObterDadosClimaticos(
                    regiao.Latitude,
                    regiao.Longitude,
                    cancellationToken);

                if (clima == null)
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, "Nao foi possivel obter os dados climaticos.");

                var associacao = idAssociacao.HasValue
                    ? await _context.Associacoes.FirstOrDefaultAsync(
                        a => a.IdAssociacao == idAssociacao.Value && a.IdRegiao == idRegiao,
                        cancellationToken)
                    : await _context.Associacoes
                        .Where(a => a.IdRegiao == idRegiao)
                        .OrderBy(a => a.IdAssociacao)
                        .FirstOrDefaultAsync(cancellationToken);

                var diagnostico = GerarDiagnosticoDetalhado(clima, cultivo);
                var orientacao = FormatarOrientacao(diagnostico, regiao.NomeRegiao, cultivo.NomeCultivo);

                if (associacao != null)
                {
                    var recomendacao = new Recomendacao
                    {
                        DataRecAsc = DateTime.UtcNow,
                        IdAssociacao = associacao.IdAssociacao,
                        IdCultivo = cultivo.IdCultivo,
                        Orientacao = orientacao,
                        TipoRecomendacao = "DIAGNOSTICO CLIMATICO",
                        NivelRisco = clima.NivelRisco,
                        ScoreRisco = clima.ScoreRisco,
                        Temperatura = clima.Temperatura,
                        Umidade = clima.Umidade,
                        VelocidadeVento = clima.VelocidadeVento,
                        Chuva = clima.Chuva,
                        FonteDados = "Open-Meteo API"
                    };

                    _context.Recomendacoes.Add(recomendacao);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                return new DiagnosticoResponseDTO
                {
                    Regiao = regiao.NomeRegiao,
                    Cultivo = cultivo.NomeCultivo,
                    IdCultivo = cultivo.IdCultivo,
                    Temperatura = clima.Temperatura,
                    Umidade = clima.Umidade,
                    Chuva = clima.Chuva,
                    VelocidadeVento = clima.VelocidadeVento,
                    ScoreRisco = clima.ScoreRisco,
                    NivelRisco = clima.NivelRisco,
                    Recomendacao = orientacao,
                    ResumoRisco = diagnostico.ResumoRisco,
                    MotivoRisco = diagnostico.MotivoRisco,
                    AcoesRecomendadas = diagnostico.AcoesRecomendadas,
                    Prioridade = diagnostico.Prioridade,
                    PrazoSugerido = diagnostico.PrazoSugerido,
                    ObservacaoTecnica = diagnostico.ObservacaoTecnica,
                    FonteDados = "Open-Meteo API"
                };
            }
            catch (HttpRequestException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "O servico climatico esta indisponivel.");
            }
            catch (JsonException)
            {
                return StatusCode(StatusCodes.Status502BadGateway, "O servico climatico retornou dados invalidos.");
            }
        }

        private static DiagnosticoDetalhado GerarDiagnosticoDetalhado(OpenMeteoResponseDTO clima, Cultivo cultivo)
        {
            var fatores = new List<string>();
            var acoes = new List<string>();
            var nomeCultivo = cultivo.NomeCultivo;

            if (clima.Chuva < 2)
            {
                fatores.Add("baixa ocorrencia de chuva");
                acoes.Add($"Reforcar a irrigacao do cultivo {nomeCultivo} e verificar a umidade do solo antes do proximo ciclo.");
            }

            if (clima.Temperatura > 35)
            {
                fatores.Add("temperatura elevada");
                acoes.Add($"Priorizar o monitoramento do cultivo {nomeCultivo}, principalmente em areas com maior exposicao ao calor.");
            }

            if (clima.Umidade < 40)
            {
                fatores.Add("umidade do ar reduzida");
                acoes.Add("Monitorar sinais de estresse hidrico nas folhas e ajustar o manejo da irrigacao.");
            }

            if (clima.VelocidadeVento > 25)
            {
                fatores.Add("velocidade do vento elevada");
                acoes.Add("Evitar pulverizacao de defensivos e proteger estruturas agricolas expostas.");
            }

            if (clima.Umidade >= 85)
            {
                fatores.Add("umidade do ar elevada");
                acoes.Add($"Inspecionar o cultivo {nomeCultivo} para prevenir fungos e doencas favorecidos pela umidade.");
            }

            if (acoes.Count == 0)
            {
                fatores.Add("condicoes climaticas dentro das faixas de acompanhamento");
                acoes.Add($"Manter o manejo planejado para o cultivo {nomeCultivo} e registrar a umidade do solo.");
                acoes.Add("Acompanhar a proxima atualizacao climatica antes de alterar irrigacao ou aplicacao de defensivos.");
            }

            if (clima.ScoreRisco >= 70 && acoes.Count < 4)
            {
                acoes.Add($"Reavaliar as condicoes climaticas nas proximas horas e registrar a resposta do cultivo {nomeCultivo}.");
            }

            var nivel = clima.NivelRisco.ToUpperInvariant();

            var prioridade = nivel switch
            {
                "ALTO" => "Alta",
                "MODERADO" => "Media",
                _ => "Baixa"
            };

            var prazo = nivel switch
            {
                "ALTO" => "Acao imediata nas proximas 24 horas.",
                "MODERADO" => "Revisar o manejo nas proximas 48 horas.",
                _ => "Manter acompanhamento no proximo ciclo de manejo."
            };

            var resumo = nivel switch
            {
                "ALTO" => $"Risco elevado para o cultivo {nomeCultivo}.",
                "MODERADO" => $"Risco moderado para o cultivo {nomeCultivo}, exigindo acompanhamento preventivo.",
                _ => $"Risco baixo para o cultivo {nomeCultivo} e condicoes favoraveis ao manejo planejado."
            };

            return new DiagnosticoDetalhado
            {
                ResumoRisco = resumo,
                MotivoRisco = $"O nivel foi definido por {JuntarFatores(fatores)}, considerando o cultivo {nomeCultivo}.",
                AcoesRecomendadas = acoes,
                Prioridade = prioridade,
                PrazoSugerido = prazo,
                ObservacaoTecnica =
                    $"Analise calculada com dados climaticos em tempo real da Open-Meteo API para o cultivo {nomeCultivo}, " +
                    "considerando temperatura, chuva, umidade e velocidade do vento."
            };
        }

        private static string FormatarOrientacao(DiagnosticoDetalhado diagnostico, string regiao, string cultivo)
        {
            var acoes = string.Join(Environment.NewLine,
                diagnostico.AcoesRecomendadas.Select((acao, index) => $"{index + 1}. {acao}"));

            return $"""
Região:
{regiao}

Cultivo:
{cultivo}

Resumo do risco:
{diagnostico.ResumoRisco}

Motivo:
{diagnostico.MotivoRisco}

Ações recomendadas:
{acoes}

Prioridade:
{diagnostico.Prioridade}

Prazo sugerido:
{diagnostico.PrazoSugerido}

Observação técnica:
{diagnostico.ObservacaoTecnica}

Fonte:
Open-Meteo API
""";
        }

        private static string JuntarFatores(IReadOnlyList<string> fatores)
        {
            return fatores.Count switch
            {
                0 => "condicoes climaticas estaveis",
                1 => fatores[0],
                2 => $"{fatores[0]} e {fatores[1]}",
                _ => $"{string.Join(", ", fatores.Take(fatores.Count - 1))} e {fatores[^1]}"
            };
        }

        private sealed class DiagnosticoDetalhado
        {
            public string ResumoRisco { get; init; } = string.Empty;
            public string MotivoRisco { get; init; } = string.Empty;
            public List<string> AcoesRecomendadas { get; init; } = [];
            public string Prioridade { get; init; } = string.Empty;
            public string PrazoSugerido { get; init; } = string.Empty;
            public string ObservacaoTecnica { get; init; } = string.Empty;
        }
    }
}