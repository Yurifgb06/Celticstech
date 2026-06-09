using Celticstech.Data;
using Celticstech.DTOs;
using Celticstech.Models;
using Celticstech.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Celticstech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegioesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CoordenadasService _coordenadasService;

        public RegioesController(AppDbContext context, CoordenadasService coordenadasService)
        {
            _context = context;
            _coordenadasService = coordenadasService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegiaoResponseDTO>>> GetRegioes()
        {
            return await _context.Regioes
                .Select(r => new RegiaoResponseDTO
                {
                    IdRegiao = r.IdRegiao,
                    NomeRegiao = r.NomeRegiao,
                    UfRegiao = r.UfRegiao,
                    Latitude = r.Latitude,
                    Longitude = r.Longitude
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RegiaoResponseDTO>> GetRegiao(int id)
        {
            var regiao = await _context.Regioes
                .Where(r => r.IdRegiao == id)
                .Select(r => new RegiaoResponseDTO
                {
                    IdRegiao = r.IdRegiao,
                    NomeRegiao = r.NomeRegiao,
                    UfRegiao = r.UfRegiao,
                    Latitude = r.Latitude,
                    Longitude = r.Longitude
                })
                .FirstOrDefaultAsync();

            if (regiao == null)
                return NotFound("Região não encontrada.");

            return regiao;
        }

        [HttpPost]
        public async Task<ActionResult<RegiaoResponseDTO>> PostRegiao(RegiaoDTO dto)
        {
            try
            {
                var coordenadas = _coordenadasService.ObterCoordenadasPorUf(dto.UfRegiao);

                var regiao = new Regiao
                {
                    NomeRegiao = dto.NomeRegiao,
                    UfRegiao = dto.UfRegiao.Trim().ToUpper(),
                    Latitude = coordenadas.Latitude,
                    Longitude = coordenadas.Longitude
                };

                _context.Regioes.Add(regiao);
                await _context.SaveChangesAsync();

                var response = new RegiaoResponseDTO
                {
                    IdRegiao = regiao.IdRegiao,
                    NomeRegiao = regiao.NomeRegiao,
                    UfRegiao = regiao.UfRegiao,
                    Latitude = regiao.Latitude,
                    Longitude = regiao.Longitude
                };

                return CreatedAtAction(nameof(GetRegiao), new { id = regiao.IdRegiao }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegiao(int id, RegiaoDTO dto)
        {
            try
            {
                var regiao = await _context.Regioes.FindAsync(id);

                if (regiao == null)
                    return NotFound("Região não encontrada.");

                var coordenadas = _coordenadasService.ObterCoordenadasPorUf(dto.UfRegiao);

                regiao.NomeRegiao = dto.NomeRegiao;
                regiao.UfRegiao = dto.UfRegiao.Trim().ToUpper();
                regiao.Latitude = coordenadas.Latitude;
                regiao.Longitude = coordenadas.Longitude;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegiao(int id)
        {
            var regiao = await _context.Regioes.FindAsync(id);

            if (regiao == null)
                return NotFound("Região não encontrada.");

            _context.Regioes.Remove(regiao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}