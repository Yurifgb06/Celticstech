using Celticstech.Data;
using Celticstech.DTOs;
using Celticstech.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Celticstech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegioesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RegioesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todas as regiões cadastradas.
        /// </summary>
        /// <returns>Lista de regiões.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegiaoResponseDTO>>> GetRegioes()
        {
            return await _context.Regioes
                .Select(r => new RegiaoResponseDTO
                {
                    IdRegiao = r.IdRegiao,
                    NomeRegiao = r.NomeRegiao,
                    UfRegiao = r.UfRegiao
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retorna uma região específica pelo ID.
        /// </summary>
        /// <param name="id">ID da região.</param>
        /// <returns>Dados da região encontrada.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RegiaoResponseDTO>> GetRegiao(int id)
        {
            var regiao = await _context.Regioes
                .Where(r => r.IdRegiao == id)
                .Select(r => new RegiaoResponseDTO
                {
                    IdRegiao = r.IdRegiao,
                    NomeRegiao = r.NomeRegiao,
                    UfRegiao = r.UfRegiao
                })
                .FirstOrDefaultAsync();

            if (regiao == null)
            {
                return NotFound("Região não encontrada.");
            }

            return regiao;
        }

        /// <summary>
        /// Cadastra uma nova região.
        /// </summary>
        /// <param name="dto">Dados da região.</param>
        /// <returns>Região criada com sucesso.</returns>
        [HttpPost]
        public async Task<ActionResult<RegiaoResponseDTO>> PostRegiao(RegiaoDTO dto)
        {
            var regiao = new Regiao
            {
                NomeRegiao = dto.NomeRegiao,
                UfRegiao = dto.UfRegiao.ToUpper()
            };

            _context.Regioes.Add(regiao);
            await _context.SaveChangesAsync();

            var response = new RegiaoResponseDTO
            {
                IdRegiao = regiao.IdRegiao,
                NomeRegiao = regiao.NomeRegiao,
                UfRegiao = regiao.UfRegiao
            };

            return CreatedAtAction(nameof(GetRegiao), new { id = regiao.IdRegiao }, response);
        }

        /// <summary>
        /// Atualiza os dados de uma região existente.
        /// </summary>
        /// <param name="id">ID da região.</param>
        /// <param name="dto">Novos dados da região.</param>
        /// <returns>Sem conteúdo em caso de sucesso.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegiao(int id, RegiaoDTO dto)
        {
            var regiao = await _context.Regioes.FindAsync(id);

            if (regiao == null)
            {
                return NotFound("Região não encontrada.");
            }

            regiao.NomeRegiao = dto.NomeRegiao;
            regiao.UfRegiao = dto.UfRegiao.ToUpper();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Remove uma região cadastrada.
        /// </summary>
        /// <param name="id">ID da região.</param>
        /// <returns>Sem conteúdo em caso de sucesso.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegiao(int id)
        {
            var regiao = await _context.Regioes.FindAsync(id);

            if (regiao == null)
            {
                return NotFound("Região não encontrada.");
            }

            _context.Regioes.Remove(regiao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}