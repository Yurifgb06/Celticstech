using System.ComponentModel.DataAnnotations;

namespace Celticstech.DTOs
{
    public class CultivoDTO
    {
        [Required]
        [MaxLength(50)]
        public string NomeCultivo { get; set; } = string.Empty;

        [Required]
        [MaxLength(40)]
        public string CategoriaCultivo { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string PorteCultivo { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string TempoColheita { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string VidaUtil { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string Intermitencia { get; set; } = string.Empty;
    }
}