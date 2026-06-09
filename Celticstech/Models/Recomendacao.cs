using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Celticstech.Models
{
    public class Recomendacao
    {
        [Key]
        public int IdRecomendacao { get; set; }

        [Required(ErrorMessage = "A data da recomendacao e obrigatoria.")]
        public DateTime DataRecAsc { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "A associacao e obrigatoria.")]
        public int IdAssociacao { get; set; }

        [ForeignKey(nameof(IdAssociacao))]
        public Associacao? Associacao { get; set; }

        [Required(ErrorMessage = "O cultivo e obrigatorio.")]
        public int IdCultivo { get; set; }

        [ForeignKey(nameof(IdCultivo))]
        public Cultivo? Cultivo { get; set; }

        [Required(ErrorMessage = "A orientacao e obrigatoria.")]
        [Column(TypeName = "text")]
        public string Orientacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tipo de recomendacao e obrigatorio.")]
        [MaxLength(30, ErrorMessage = "O tipo de recomendacao deve ter no maximo 30 caracteres.")]
        public string TipoRecomendacao { get; set; } = string.Empty;

        [MaxLength(30, ErrorMessage = "O nivel de risco deve ter no maximo 30 caracteres.")]
        public string? NivelRisco { get; set; }

        [Range(0, 100, ErrorMessage = "O score de risco deve estar entre 0 e 100.")]
        public int? ScoreRisco { get; set; }

        public double? Temperatura { get; set; }

        public double? Umidade { get; set; }

        public double? VelocidadeVento { get; set; }

        public double? Chuva { get; set; }

        [MaxLength(100, ErrorMessage = "A fonte dos dados deve ter no maximo 100 caracteres.")]
        public string? FonteDados { get; set; }
    }
}
