using Celticstech.Data;
using Celticstech.DTOs;
using Celticstech.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Celticstech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecomendacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecomendacoesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todas as recomendações cadastradas.
        /// </summary>
        /// <returns>Lista de recomendações com associação e cultivo relacionados.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecomendacaoResponseDTO>>> GetRecomendacoes()
        {
            return await _context.Recomendacoes
                .Include(r => r.Associacao)
                .Include(r => r.Cultivo)
                .Select(r => new RecomendacaoResponseDTO
                {
                    IdRecomendacao = r.IdRecomendacao,
                    DataRecAsc = r.DataRecAsc,
                    IdAssociacao = r.IdAssociacao,
                    NomeAssociacao = r.Associacao != null ? r.Associacao.NomeAssociacao : "",
                    IdCultivo = r.IdCultivo,
                    NomeCultivo = r.Cultivo != null ? r.Cultivo.NomeCultivo : "",
                    CategoriaCultivo = r.Cultivo != null ? r.Cultivo.CategoriaCultivo : "",
                    PorteCultivo = r.Cultivo != null ? r.Cultivo.PorteCultivo : "",
                    Orientacao = r.Orientacao,
                    TipoRecomendacao = r.TipoRecomendacao
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retorna uma recomendação específica pelo ID.
        /// </summary>
        /// <param name="id">ID da recomendação.</param>
        /// <returns>Dados da recomendação encontrada.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RecomendacaoResponseDTO>> GetRecomendacao(int id)
        {
            var recomendacao = await _context.Recomendacoes
                .Include(r => r.Associacao)
                .Include(r => r.Cultivo)
                .Where(r => r.IdRecomendacao == id)
                .Select(r => new RecomendacaoResponseDTO
                {
                    IdRecomendacao = r.IdRecomendacao,
                    DataRecAsc = r.DataRecAsc,
                    IdAssociacao = r.IdAssociacao,
                    NomeAssociacao = r.Associacao != null ? r.Associacao.NomeAssociacao : "",
                    IdCultivo = r.IdCultivo,
                    NomeCultivo = r.Cultivo != null ? r.Cultivo.NomeCultivo : "",
                    CategoriaCultivo = r.Cultivo != null ? r.Cultivo.CategoriaCultivo : "",
                    PorteCultivo = r.Cultivo != null ? r.Cultivo.PorteCultivo : "",
                    Orientacao = r.Orientacao,
                    TipoRecomendacao = r.TipoRecomendacao
                })
                .FirstOrDefaultAsync();

            if (recomendacao == null)
            {
                return NotFound("Recomendação não encontrada.");
            }

            return recomendacao;
        }

        /// <summary>
        /// Gera automaticamente uma recomendação agrícola com base no cultivo informado.
        /// </summary>
        /// <param name="dto">Dados necessários para geração da recomendação.</param>
        /// <returns>Recomendação criada automaticamente.</returns>
        [HttpPost]
        public async Task<ActionResult<RecomendacaoResponseDTO>> PostRecomendacao(RecomendacaoDTO dto)
        {
            var associacao = await _context.Associacoes
                .FirstOrDefaultAsync(a => a.IdAssociacao == dto.IdAssociacao);

            if (associacao == null)
            {
                return BadRequest("A associação informada não existe.");
            }

            var cultivo = await _context.Cultivos
                .FirstOrDefaultAsync(c => c.IdCultivo == dto.IdCultivo);

            if (cultivo == null)
            {
                return BadRequest("O cultivo informado não existe.");
            }

            var recomendacaoGerada = GerarRecomendacaoPorCultivo(cultivo.NomeCultivo);

            var recomendacao = new Recomendacao
            {
                DataRecAsc = dto.DataRecAsc,
                IdAssociacao = dto.IdAssociacao,
                IdCultivo = dto.IdCultivo,
                Orientacao = recomendacaoGerada.Orientacao,
                TipoRecomendacao = recomendacaoGerada.Tipo
            };

            _context.Recomendacoes.Add(recomendacao);
            await _context.SaveChangesAsync();

            var response = new RecomendacaoResponseDTO
            {
                IdRecomendacao = recomendacao.IdRecomendacao,
                DataRecAsc = recomendacao.DataRecAsc,
                IdAssociacao = associacao.IdAssociacao,
                NomeAssociacao = associacao.NomeAssociacao,
                IdCultivo = cultivo.IdCultivo,
                NomeCultivo = cultivo.NomeCultivo,
                CategoriaCultivo = cultivo.CategoriaCultivo,
                PorteCultivo = cultivo.PorteCultivo,
                Orientacao = recomendacao.Orientacao,
                TipoRecomendacao = recomendacao.TipoRecomendacao
            };

            return CreatedAtAction(nameof(GetRecomendacao),
                new { id = recomendacao.IdRecomendacao },
                response);
        }

        /// <summary>
        /// Atualiza uma recomendação existente e recalcula a orientação com base no cultivo.
        /// </summary>
        /// <param name="id">ID da recomendação.</param>
        /// <param name="dto">Novos dados da recomendação.</param>
        /// <returns>Sem conteúdo em caso de sucesso.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecomendacao(int id, RecomendacaoDTO dto)
        {
            var recomendacao = await _context.Recomendacoes.FindAsync(id);

            if (recomendacao == null)
            {
                return NotFound("Recomendação não encontrada.");
            }

            var associacaoExiste = await _context.Associacoes
                .AnyAsync(a => a.IdAssociacao == dto.IdAssociacao);

            if (!associacaoExiste)
            {
                return BadRequest("A associação informada não existe.");
            }

            var cultivo = await _context.Cultivos
                .FirstOrDefaultAsync(c => c.IdCultivo == dto.IdCultivo);

            if (cultivo == null)
            {
                return BadRequest("O cultivo informado não existe.");
            }

            var recomendacaoGerada = GerarRecomendacaoPorCultivo(cultivo.NomeCultivo);

            recomendacao.DataRecAsc = dto.DataRecAsc;
            recomendacao.IdAssociacao = dto.IdAssociacao;
            recomendacao.IdCultivo = dto.IdCultivo;
            recomendacao.Orientacao = recomendacaoGerada.Orientacao;
            recomendacao.TipoRecomendacao = recomendacaoGerada.Tipo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Remove uma recomendação cadastrada.
        /// </summary>
        /// <param name="id">ID da recomendação.</param>
        /// <returns>Sem conteúdo em caso de sucesso.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecomendacao(int id)
        {
            var recomendacao = await _context.Recomendacoes.FindAsync(id);

            if (recomendacao == null)
            {
                return NotFound("Recomendação não encontrada.");
            }

            _context.Recomendacoes.Remove(recomendacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static (string Tipo, string Orientacao) GerarRecomendacaoPorCultivo(string nomeCultivo)
        {
            var cultivo = nomeCultivo.Trim().ToUpper();

            return cultivo switch
            {
                "SOJA" => ("IRRIGACAO", "Para soja, recomenda-se monitorar a umidade do solo e reforçar a irrigação em períodos de estiagem."),
                "MILHO" => ("IRRIGACAO", "Para milho, recomenda-se atenção ao déficit hídrico, principalmente nas fases de crescimento e formação dos grãos."),
                "ALGODAO" or "ALGODÃO" => ("COLHEITA", "Para algodão, recomenda-se acompanhar o clima seco e planejar a colheita em períodos com menor umidade."),
                "MANGA" => ("IRRIGACAO", "Para manga, recomenda-se irrigação controlada e monitoramento de altas temperaturas no período de frutificação."),
                "UVA" => ("IRRIGACAO", "Para uva, recomenda-se controle frequente da irrigação e atenção à temperatura para manter a qualidade dos frutos."),
                "CANA" or "CANA-DE-AÇÚCAR" or "CANA-DE-ACUCAR" => ("NÃO IRRIGAR", "Para cana-de-açúcar, recomenda-se evitar irrigação excessiva e acompanhar o volume de chuvas da região."),
                "CACAU" => ("COLHEITA", "Para cacau, recomenda-se acompanhar a umidade e realizar manejo adequado de sombra antes da colheita."),
                "CAJU" => ("NÃO IRRIGAR", "Para caju, recomenda-se atenção aos períodos de seca, pois a cultura possui maior resistência hídrica."),
                "FEIJAO" or "FEIJÃO" => ("IRRIGACAO", "Para feijão, recomenda-se irrigação moderada e atenção ao ciclo curto da cultura."),
                "MANDIOCA" => ("COLHEITA", "Para mandioca, recomenda-se acompanhar o tempo de cultivo e planejar a colheita conforme o desenvolvimento da raiz."),
                _ => ("IRRIGACAO", "Cultivo cadastrado sem regra específica. Recomenda-se monitorar clima, solo e necessidade de irrigação.")
            };
        }
    }
}