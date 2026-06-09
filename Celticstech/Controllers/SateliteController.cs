using Celticstech.Data;
using Celticstech.DTOs;
using Celticstech.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Celticstech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SateliteController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly OpenMeteoService _openMeteoService;

        public SateliteController(
            AppDbContext context,
            OpenMeteoService openMeteoService)
        {
            _context = context;
            _openMeteoService = openMeteoService;
        }

        /// <summary>
        /// Consulta os dados climaticos atuais de uma regiao cadastrada.
        /// </summary>
        [HttpGet("clima/regiao/{id:int}")]
        public async Task<ActionResult<ClimaRegiaoResponseDTO>> GetClimaRegiao(
            int id,
            CancellationToken cancellationToken)
        {
            var regiao = await _context.Regioes
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.IdRegiao == id, cancellationToken);

            if (regiao == null)
            {
                return NotFound("Regiao nao encontrada.");
            }

            try
            {
                var clima = await _openMeteoService.ObterDadosClimaticos(
                    regiao.Latitude,
                    regiao.Longitude,
                    cancellationToken);

                if (clima == null)
                {
                    return StatusCode(
                        StatusCodes.Status503ServiceUnavailable,
                        "Nao foi possivel obter os dados climaticos.");
                }

                return new ClimaRegiaoResponseDTO
                {
                    IdRegiao = regiao.IdRegiao,
                    Regiao = regiao.NomeRegiao,
                    Uf = regiao.UfRegiao,
                    Latitude = regiao.Latitude,
                    Longitude = regiao.Longitude,
                    Temperatura = clima.Temperatura,
                    Umidade = clima.Umidade,
                    VelocidadeVento = clima.VelocidadeVento,
                    Chuva = clima.Chuva,
                    ScoreRisco = clima.ScoreRisco,
                    NivelRisco = clima.NivelRisco
                };
            }
            catch (HttpRequestException)
            {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    "O servico climatico esta indisponivel.");
            }
            catch (JsonException)
            {
                return StatusCode(
                    StatusCodes.Status502BadGateway,
                    "O servico climatico retornou dados invalidos.");
            }
        }
    }
}
