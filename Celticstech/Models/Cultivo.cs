using System.ComponentModel.DataAnnotations;

namespace Celticstech.Models
{
    public class Cultivo
    {
        [Key]
        public int IdCultivo { get; set; }

        [Required(ErrorMessage = "O nome do cultivo e obrigatorio.")]
        [MaxLength(50, ErrorMessage = "O nome do cultivo deve ter no maximo 50 caracteres.")]
        public string NomeCultivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A categoria do cultivo e obrigatoria.")]
        [MaxLength(40, ErrorMessage = "A categoria deve ter no maximo 40 caracteres.")]
        public string CategoriaCultivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O porte do cultivo e obrigatorio.")]
        [MaxLength(20, ErrorMessage = "O porte deve ter no maximo 20 caracteres.")]
        public string PorteCultivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tempo de colheita e obrigatorio.")]
        [MaxLength(30, ErrorMessage = "O tempo de colheita deve ter no maximo 30 caracteres.")]
        public string TempoColheita { get; set; } = string.Empty;

        [Required(ErrorMessage = "A vida util e obrigatoria.")]
        [MaxLength(30, ErrorMessage = "A vida util deve ter no maximo 30 caracteres.")]
        public string VidaUtil { get; set; } = string.Empty;

        [Required(ErrorMessage = "A intermitencia e obrigatoria.")]
        [MaxLength(30, ErrorMessage = "A intermitencia deve ter no maximo 30 caracteres.")]
        public string Intermitencia { get; set; } = string.Empty;

        public ICollection<Agricultor> Agricultores { get; set; } = new List<Agricultor>();

        public ICollection<Recomendacao> Recomendacoes { get; set; } = new List<Recomendacao>();
    }
}