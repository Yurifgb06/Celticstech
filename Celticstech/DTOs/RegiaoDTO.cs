using System.ComponentModel.DataAnnotations;

namespace Celticstech.DTOs
{
    public class RegiaoDTO
    {
        [Required]
        [MaxLength(50)]
        public string NomeRegiao { get; set; } = string.Empty;

        [Required]
        [MaxLength(2)]
        public string UfRegiao { get; set; } = string.Empty;
    }
}