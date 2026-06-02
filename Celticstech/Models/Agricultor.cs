using System.ComponentModel.DataAnnotations;

namespace Celticstech.Models
{
    public class Agricultor
    {
        [Key]
        public int IdAgricultor { get; set; }

        [Required(ErrorMessage = "O nome do agricultor é obrigatório.")]
        [MaxLength(80, ErrorMessage = "O nome do agricultor deve ter no máximo 80 caracteres.")]
        public string NomeAgricultor { get; set; } = string.Empty;

        [Required(ErrorMessage = "A idade é obrigatória.")]
        [Range(18, 120, ErrorMessage = "A idade deve estar entre 18 e 120 anos.")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "O sexo é obrigatório.")]
        [MaxLength(1, ErrorMessage = "O sexo deve ter apenas 1 caractere.")]
        public string Sexo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A quantidade de dependentes é obrigatória.")]
        [Range(0, 50, ErrorMessage = "A quantidade de dependentes deve estar entre 0 e 50.")]
        public int QtdeDependentes { get; set; }
    }
}