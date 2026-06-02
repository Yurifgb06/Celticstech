using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Celticstech.Models
{
    public class Recomendacao
    {
        [Key]
        public int IdRecomendacao { get; set; }

        [Required(ErrorMessage = "A data da recomendação é obrigatória.")]
        public DateTime DataRecAsc { get; set; }

        [Required(ErrorMessage = "A associação é obrigatória.")]
        public int IdAssociacao { get; set; }

        [ForeignKey("IdAssociacao")]
        public Associacao? Associacao { get; set; }

        [Required(ErrorMessage = "O cultivo é obrigatório.")]
        public int IdCultivo { get; set; }

        [ForeignKey("IdCultivo")]
        public Cultivo? Cultivo { get; set; }

        [Required(ErrorMessage = "A orientação é obrigatória.")]
        [MaxLength(300, ErrorMessage = "A orientação deve ter no máximo 300 caracteres.")]
        public string Orientacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tipo de recomendação é obrigatório.")]
        [MaxLength(30, ErrorMessage = "O tipo de recomendação deve ter no máximo 30 caracteres.")]
        public string TipoRecomendacao { get; set; } = string.Empty;
    }
}