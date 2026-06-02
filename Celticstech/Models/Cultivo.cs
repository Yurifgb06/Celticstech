using System.ComponentModel.DataAnnotations;

namespace Celticstech.Models
{
    public class Cultivo
    {
        [Key]
        public int IdCultivo { get; set; }

        [Required(ErrorMessage = "O nome do cultivo é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O nome do cultivo deve ter no máximo 50 caracteres.")]
        public string NomeCultivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A categoria do cultivo é obrigatória.")]
        [MaxLength(40, ErrorMessage = "A categoria do cultivo deve ter no máximo 40 caracteres.")]
        public string CategoriaCultivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O porte do cultivo é obrigatório.")]
        [MaxLength(20, ErrorMessage = "O porte do cultivo deve ter no máximo 20 caracteres.")]
        public string PorteCultivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tempo de colheita é obrigatório.")]
        [MaxLength(30, ErrorMessage = "O tempo de colheita deve ter no máximo 30 caracteres.")]
        public string TempoColheita { get; set; } = string.Empty;

        [Required(ErrorMessage = "A vida útil é obrigatória.")]
        [MaxLength(30, ErrorMessage = "A vida útil deve ter no máximo 30 caracteres.")]
        public string VidaUtil { get; set; } = string.Empty;

        [Required(ErrorMessage = "A intermitência é obrigatória.")]
        [MaxLength(30, ErrorMessage = "A intermitência deve ter no máximo 30 caracteres.")]
        public string Intermitencia { get; set; } = string.Empty;

        public ICollection<Recomendacao> Recomendacoes { get; set; } = new List<Recomendacao>();
    }
}