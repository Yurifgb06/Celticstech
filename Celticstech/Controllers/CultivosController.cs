using Celticstech.Data;
using Celticstech.DTOs;
using Celticstech.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Celticstech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CultivosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CultivosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cultivo>>> GetCultivos()
        {
            return await _context.Cultivos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cultivo>> GetCultivo(int id)
        {
            var cultivo = await _context.Cultivos.FindAsync(id);

            if (cultivo == null)
            {
                return NotFound("Cultivo não encontrado.");
            }

            return cultivo;
        }

        [HttpPost]
        public async Task<ActionResult<Cultivo>> PostCultivo(CultivoDTO dto)
        {
            var portesValidos = new[] { "ARBUSTO", "RAIZ", "ARVORE", "HORTALICA" };

            if (!portesValidos.Contains(dto.PorteCultivo.ToUpper()))
            {
                return BadRequest("Porte do cultivo inválido. Use: ARBUSTO, RAIZ, ARVORE ou HORTALICA.");
            }

            var cultivo = new Cultivo
            {
                NomeCultivo = dto.NomeCultivo,
                CategoriaCultivo = dto.CategoriaCultivo,
                PorteCultivo = dto.PorteCultivo.ToUpper(),
                TempoColheita = dto.TempoColheita,
                VidaUtil = dto.VidaUtil,
                Intermitencia = dto.Intermitencia
            };

            _context.Cultivos.Add(cultivo);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCultivo),
                new { id = cultivo.IdCultivo },
                cultivo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCultivo(int id, CultivoDTO dto)
        {
            var cultivo = await _context.Cultivos.FindAsync(id);

            if (cultivo == null)
            {
                return NotFound("Cultivo não encontrado.");
            }

            var portesValidos = new[] { "ARBUSTO", "RAIZ", "ARVORE", "HORTALICA" };

            if (!portesValidos.Contains(dto.PorteCultivo.ToUpper()))
            {
                return BadRequest("Porte do cultivo inválido. Use: ARBUSTO, RAIZ, ARVORE ou HORTALICA.");
            }

            cultivo.NomeCultivo = dto.NomeCultivo;
            cultivo.CategoriaCultivo = dto.CategoriaCultivo;
            cultivo.PorteCultivo = dto.PorteCultivo.ToUpper();
            cultivo.TempoColheita = dto.TempoColheita;
            cultivo.VidaUtil = dto.VidaUtil;
            cultivo.Intermitencia = dto.Intermitencia;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCultivo(int id)
        {
            var cultivo = await _context.Cultivos.FindAsync(id);

            if (cultivo == null)
            {
                return NotFound("Cultivo não encontrado.");
            }

            _context.Cultivos.Remove(cultivo);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}