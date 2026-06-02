using System.ComponentModel.DataAnnotations;

namespace Celticstech.DTOs
{
    public class AssociacaoDTO
    {
        [Required]
        [MaxLength(120)]
        public string NomeAssociacao { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string SiglaAssociacao { get; set; } = string.Empty;

        [Required]
        public int IdRegiao { get; set; }

        [Required]
        [MaxLength(14)]
        public string Cnpj { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [MaxLength(60)]
        public string Senha { get; set; } = string.Empty;
    }
}