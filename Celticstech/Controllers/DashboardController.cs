using Celticstech.Data;
using Celticstech.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Celticstech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna os principais indicadores operacionais da plataforma.
        /// </summary>
        [HttpGet("resumo")]
        public async Task<ActionResult<DashboardResumoResponseDTO>> GetResumo(
            CancellationToken cancellationToken)
        {
            return new DashboardResumoResponseDTO
            {
                TotalRegioes = await _context.Regioes.CountAsync(cancellationToken),
                TotalAssociacoes = await _context.Associacoes.CountAsync(cancellationToken),
                TotalCultivos = await _context.Cultivos.CountAsync(cancellationToken),
                TotalAgricultores = await _context.Agricultores.CountAsync(cancellationToken),
                TotalRecomendacoes = await _context.Recomendacoes.CountAsync(cancellationToken)
            };
        }
    }
}