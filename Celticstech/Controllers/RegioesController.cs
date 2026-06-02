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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Regiao>>> GetRegioes()
        {
            return await _context.Regioes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Regiao>> GetRegiao(int id)
        {
            var regiao = await _context.Regioes.FindAsync(id);

            if (regiao == null)
            {
                return NotFound("Região não encontrada.");
            }

            return regiao;
        }

        [HttpPost]
        public async Task<ActionResult<Regiao>> PostRegiao(RegiaoDTO dto)
        {
            var regiao = new Regiao
            {
                NomeRegiao = dto.NomeRegiao,
                UfRegiao = dto.UfRegiao.ToUpper()
            };

            _context.Regioes.Add(regiao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRegiao), new { id = regiao.IdRegiao }, regiao);
        }

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