using System.ComponentModel.DataAnnotations;

namespace Celticstech.DTOs
{
    public class RecomendacaoDTO
    {
        [Required]
        public DateTime DataRecAsc { get; set; }

        [Required]
        public int IdAssociacao { get; set; }

        [Required]
        public int IdCultivo { get; set; }
    }
}