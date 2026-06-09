using Celticstech.Data;
using Celticstech.DTOs;
using Celticstech.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Celticstech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AssociacoesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todas as associações cadastradas.
        /// </summary>
        /// <returns>Lista de associações sem exibir a senha.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssociacaoResponseDTO>>> GetAssociacoes()
        {
            return await _context.Associacoes
                .Include(a => a.Regiao)
                .Select(a => new AssociacaoResponseDTO
                {
                    IdAssociacao = a.IdAssociacao,
                    NomeAssociacao = a.NomeAssociacao,
                    SiglaAssociacao = a.SiglaAssociacao,
                    IdRegiao = a.IdRegiao,
                    NomeRegiao = a.Regiao != null ? a.Regiao.NomeRegiao : null,
                    Cnpj = a.Cnpj,
                    Login = a.Login
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retorna uma associação específica pelo ID.
        /// </summary>
        /// <param name="id">ID da associação.</param>
        /// <returns>Dados da associação encontrada.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AssociacaoResponseDTO>> GetAssociacao(int id)
        {
            var associacao = await _context.Associacoes
                .Include(a => a.Regiao)
                .Where(a => a.IdAssociacao == id)
                .Select(a => new AssociacaoResponseDTO
                {
                    IdAssociacao = a.IdAssociacao,
                    NomeAssociacao = a.NomeAssociacao,
                    SiglaAssociacao = a.SiglaAssociacao,
                    IdRegiao = a.IdRegiao,
                    NomeRegiao = a.Regiao != null ? a.Regiao.NomeRegiao : null,
                    Cnpj = a.Cnpj,
                    Login = a.Login
                })
                .FirstOrDefaultAsync();

            if (associacao == null)
            {
                return NotFound("Associação não encontrada.");
            }

            return associacao;
        }

        /// <summary>
        /// Cadastra uma nova associação.
        /// </summary>
        /// <param name="dto">Dados da associação.</param>
        /// <returns>Associação criada com sucesso.</returns>
        [HttpPost]
        public async Task<ActionResult<AssociacaoResponseDTO>> PostAssociacao(AssociacaoDTO dto)
        {
            var regiaoExiste = await _context.Regioes.AnyAsync(r => r.IdRegiao == dto.IdRegiao);

            if (!regiaoExiste)
            {
                return BadRequest("A região informada não existe.");
            }

            var associacao = new Associacao
            {
                NomeAssociacao = dto.NomeAssociacao,
                SiglaAssociacao = dto.SiglaAssociacao,
                IdRegiao = dto.IdRegiao,
                Cnpj = dto.Cnpj,
                Login = dto.Login,
                Senha = dto.Senha
            };

            _context.Associacoes.Add(associacao);
            await _context.SaveChangesAsync();

            var response = new AssociacaoResponseDTO
            {
                IdAssociacao = associacao.IdAssociacao,
                NomeAssociacao = associacao.NomeAssociacao,
                SiglaAssociacao = associacao.SiglaAssociacao,
                IdRegiao = associacao.IdRegiao,
                Cnpj = associacao.Cnpj,
                Login = associacao.Login
            };

            return CreatedAtAction(nameof(GetAssociacao),
                new { id = associacao.IdAssociacao },
                response);
        }

        /// <summary>
        /// Atualiza os dados de uma associação existente.
        /// </summary>
        /// <param name="id">ID da associação.</param>
        /// <param name="dto">Novos dados da associação.</param>
        /// <returns>Sem conteúdo em caso de sucesso.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssociacao(int id, AssociacaoDTO dto)
        {
            var associacao = await _context.Associacoes.FindAsync(id);

            if (associacao == null)
            {
                return NotFound("Associação não encontrada.");
            }

            var regiaoExiste = await _context.Regioes.AnyAsync(r => r.IdRegiao == dto.IdRegiao);

            if (!regiaoExiste)
            {
                return BadRequest("A região informada não existe.");
            }

            associacao.NomeAssociacao = dto.NomeAssociacao;
            associacao.SiglaAssociacao = dto.SiglaAssociacao;
            associacao.IdRegiao = dto.IdRegiao;
            associacao.Cnpj = dto.Cnpj;
            associacao.Login = dto.Login;
            associacao.Senha = dto.Senha;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Remove uma associação cadastrada.
        /// </summary>
        /// <param name="id">ID da associação.</param>
        /// <returns>Sem conteúdo em caso de sucesso.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssociacao(int id)
        {
            var associacao = await _context.Associacoes.FindAsync(id);

            if (associacao == null)
            {
                return NotFound("Associação não encontrada.");
            }

            _context.Associacoes.Remove(associacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}