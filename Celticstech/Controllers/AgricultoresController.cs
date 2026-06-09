using Celticstech.Data;
using Celticstech.DTOs;
using Celticstech.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Celticstech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgricultoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AgricultoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgricultorResponseDTO>>> GetAgricultores()
        {
            return await _context.Agricultores
                .Include(a => a.Associacao)
                .Include(a => a.Cultivo)
                .Select(a => new AgricultorResponseDTO
                {
                    IdAgricultor = a.IdAgricultor,
                    NomeAgricultor = a.NomeAgricultor,
                    Cpf = a.Cpf,
                    Telefone = a.Telefone,
                    IdAssociacao = a.IdAssociacao,
                    NomeAssociacao = a.Associacao != null ? a.Associacao.NomeAssociacao : string.Empty,
                    IdCultivo = a.IdCultivo,
                    NomeCultivo = a.Cultivo != null ? a.Cultivo.NomeCultivo : string.Empty
                })
                .ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AgricultorResponseDTO>> GetAgricultor(int id)
        {
            var agricultor = await _context.Agricultores
                .Include(a => a.Associacao)
                .Include(a => a.Cultivo)
                .Where(a => a.IdAgricultor == id)
                .Select(a => new AgricultorResponseDTO
                {
                    IdAgricultor = a.IdAgricultor,
                    NomeAgricultor = a.NomeAgricultor,
                    Cpf = a.Cpf,
                    Telefone = a.Telefone,
                    IdAssociacao = a.IdAssociacao,
                    NomeAssociacao = a.Associacao != null ? a.Associacao.NomeAssociacao : string.Empty,
                    IdCultivo = a.IdCultivo,
                    NomeCultivo = a.Cultivo != null ? a.Cultivo.NomeCultivo : string.Empty
                })
                .FirstOrDefaultAsync();

            if (agricultor == null)
                return NotFound("Agricultor nao encontrado.");

            return agricultor;
        }

        [HttpPost]
        public async Task<ActionResult<AgricultorResponseDTO>> PostAgricultor(AgricultorDTO dto)
        {
            var associacao = await _context.Associacoes.FindAsync(dto.IdAssociacao);
            if (associacao == null)
                return BadRequest("A associacao informada nao existe.");

            var cultivo = await _context.Cultivos.FindAsync(dto.IdCultivo);
            if (cultivo == null)
                return BadRequest("O cultivo informado nao existe.");

            var cpfJaExiste = await _context.Agricultores.AnyAsync(a => a.Cpf == dto.Cpf);
            if (cpfJaExiste)
                return BadRequest("Ja existe um agricultor cadastrado com este CPF.");

            var agricultor = new Agricultor
            {
                NomeAgricultor = dto.NomeAgricultor,
                Cpf = dto.Cpf,
                Telefone = dto.Telefone,
                IdAssociacao = dto.IdAssociacao,
                IdCultivo = dto.IdCultivo
            };

            _context.Agricultores.Add(agricultor);
            await _context.SaveChangesAsync();

            var response = new AgricultorResponseDTO
            {
                IdAgricultor = agricultor.IdAgricultor,
                NomeAgricultor = agricultor.NomeAgricultor,
                Cpf = agricultor.Cpf,
                Telefone = agricultor.Telefone,
                IdAssociacao = agricultor.IdAssociacao,
                NomeAssociacao = associacao.NomeAssociacao,
                IdCultivo = agricultor.IdCultivo,
                NomeCultivo = cultivo.NomeCultivo
            };

            return CreatedAtAction(nameof(GetAgricultor), new { id = agricultor.IdAgricultor }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAgricultor(int id, AgricultorDTO dto)
        {
            var agricultor = await _context.Agricultores.FindAsync(id);
            if (agricultor == null)
                return NotFound("Agricultor nao encontrado.");

            var associacaoExiste = await _context.Associacoes.AnyAsync(a => a.IdAssociacao == dto.IdAssociacao);
            if (!associacaoExiste)
                return BadRequest("A associacao informada nao existe.");

            var cultivoExiste = await _context.Cultivos.AnyAsync(c => c.IdCultivo == dto.IdCultivo);
            if (!cultivoExiste)
                return BadRequest("O cultivo informado nao existe.");

            var cpfJaExiste = await _context.Agricultores
                .AnyAsync(a => a.Cpf == dto.Cpf && a.IdAgricultor != id);
            if (cpfJaExiste)
                return BadRequest("Ja existe outro agricultor cadastrado com este CPF.");

            agricultor.NomeAgricultor = dto.NomeAgricultor;
            agricultor.Cpf = dto.Cpf;
            agricultor.Telefone = dto.Telefone;
            agricultor.IdAssociacao = dto.IdAssociacao;
            agricultor.IdCultivo = dto.IdCultivo;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAgricultor(int id)
        {
            var agricultor = await _context.Agricultores.FindAsync(id);
            if (agricultor == null)
                return NotFound("Agricultor nao encontrado.");

            _context.Agricultores.Remove(agricultor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
