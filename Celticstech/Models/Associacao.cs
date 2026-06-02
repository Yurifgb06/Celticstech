using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Celticstech.Models
{
    public class Associacao
    {
        [Key]
        public int IdAssociacao { get; set; }

        [Required(ErrorMessage = "O nome da associação é obrigatório.")]
        [MaxLength(120, ErrorMessage = "O nome da associação deve ter no máximo 120 caracteres.")]
        public string NomeAssociacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A sigla da associação é obrigatória.")]
        [MaxLength(10, ErrorMessage = "A sigla deve ter no máximo 10 caracteres.")]
        public string SiglaAssociacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A região é obrigatória.")]
        public int IdRegiao { get; set; }

        [ForeignKey("IdRegiao")]
        public Regiao? Regiao { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [MaxLength(14, ErrorMessage = "O CNPJ deve ter no máximo 14 caracteres.")]
        public string Cnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "O login é obrigatório.")]
        [MaxLength(30, ErrorMessage = "O login deve ter no máximo 30 caracteres.")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MaxLength(60, ErrorMessage = "A senha deve ter no máximo 60 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        public ICollection<Recomendacao> Recomendacoes { get; set; } = new List<Recomendacao>();
    }
}