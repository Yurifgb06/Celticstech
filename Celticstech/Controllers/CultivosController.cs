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

        /// <summary>
        /// Retorna todos os cultivos cadastrados.
        /// </summary>
        /// <returns>Lista de cultivos.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CultivoResponseDTO>>> GetCultivos()
        {
            return await _context.Cultivos
                .Select(c => new CultivoResponseDTO
                {
                    IdCultivo = c.IdCultivo,
                    NomeCultivo = c.NomeCultivo,
                    CategoriaCultivo = c.CategoriaCultivo,
                    PorteCultivo = c.PorteCultivo,
                    TempoColheita = c.TempoColheita,
                    VidaUtil = c.VidaUtil,
                    Intermitencia = c.Intermitencia
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retorna um cultivo específico pelo ID.
        /// </summary>
        /// <param name="id">ID do cultivo.</param>
        /// <returns>Dados do cultivo encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CultivoResponseDTO>> GetCultivo(int id)
        {
            var cultivo = await _context.Cultivos
                .Where(c => c.IdCultivo == id)
                .Select(c => new CultivoResponseDTO
                {
                    IdCultivo = c.IdCultivo,
                    NomeCultivo = c.NomeCultivo,
                    CategoriaCultivo = c.CategoriaCultivo,
                    PorteCultivo = c.PorteCultivo,
                    TempoColheita = c.TempoColheita,
                    VidaUtil = c.VidaUtil,
                    Intermitencia = c.Intermitencia
                })
                .FirstOrDefaultAsync();

            if (cultivo == null)
            {
                return NotFound("Cultivo não encontrado.");
            }

            return cultivo;
        }

        /// <summary>
        /// Cadastra um novo cultivo.
        /// </summary>
        /// <param name="dto">Dados do cultivo.</param>
        /// <returns>Cultivo criado com sucesso.</returns>
        [HttpPost]
        public async Task<ActionResult<CultivoResponseDTO>> PostCultivo(CultivoDTO dto)
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

            var response = new CultivoResponseDTO
            {
                IdCultivo = cultivo.IdCultivo,
                NomeCultivo = cultivo.NomeCultivo,
                CategoriaCultivo = cultivo.CategoriaCultivo,
                PorteCultivo = cultivo.PorteCultivo,
                TempoColheita = cultivo.TempoColheita,
                VidaUtil = cultivo.VidaUtil,
                Intermitencia = cultivo.Intermitencia
            };

            return CreatedAtAction(nameof(GetCultivo),
                new { id = cultivo.IdCultivo },
                response);
        }

        /// <summary>
        /// Atualiza os dados de um cultivo existente.
        /// </summary>
        /// <param name="id">ID do cultivo.</param>
        /// <param name="dto">Novos dados do cultivo.</param>
        /// <returns>Sem conteúdo em caso de sucesso.</returns>
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

        /// <summary>
        /// Remove um cultivo cadastrado.
        /// </summary>
        /// <param name="id">ID do cultivo.</param>
        /// <returns>Sem conteúdo em caso de sucesso.</returns>
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