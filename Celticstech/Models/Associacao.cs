using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Celticstech.Models
{
    public class Associacao
    {
        [Key]
        public int IdAssociacao { get; set; }

        [Required(ErrorMessage = "O nome da associacao e obrigatorio.")]
        [MaxLength(120, ErrorMessage = "O nome da associacao deve ter no maximo 120 caracteres.")]
        public string NomeAssociacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A sigla da associacao e obrigatoria.")]
        [MaxLength(10, ErrorMessage = "A sigla deve ter no maximo 10 caracteres.")]
        public string SiglaAssociacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A regiao e obrigatoria.")]
        public int IdRegiao { get; set; }

        [ForeignKey(nameof(IdRegiao))]
        public Regiao? Regiao { get; set; }

        [Required(ErrorMessage = "O CNPJ da associacao e obrigatorio.")]
        [MaxLength(14, ErrorMessage = "O CNPJ deve ter no maximo 14 caracteres.")]
        public string Cnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "O login da associacao e obrigatorio.")]
        [MaxLength(30, ErrorMessage = "O login deve ter no maximo 30 caracteres.")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha da associacao e obrigatoria.")]
        [MaxLength(60, ErrorMessage = "A senha deve ter no maximo 60 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        public ICollection<Agricultor> Agricultores { get; set; } = new List<Agricultor>();

        public ICollection<Recomendacao> Recomendacoes { get; set; } = new List<Recomendacao>();
    }
}