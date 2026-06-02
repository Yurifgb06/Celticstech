using System.ComponentModel.DataAnnotations;

namespace Celticstech.Models
{
    public class Regiao
    {
        [Key]
        public int IdRegiao { get; set; }

        [Required(ErrorMessage = "O nome da região é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O nome da região deve ter no máximo 50 caracteres.")]
        public string NomeRegiao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A UF da região é obrigatória.")]
        [MaxLength(2, ErrorMessage = "A UF deve ter no máximo 2 caracteres.")]
        public string UfRegiao { get; set; } = string.Empty;

        public ICollection<Associacao> Associacoes { get; set; } = new List<Associacao>();
    }
}