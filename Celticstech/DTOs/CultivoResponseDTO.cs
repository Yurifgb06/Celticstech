namespace Celticstech.DTOs
{
    public class CultivoResponseDTO
    {
        public int IdCultivo { get; set; }

        public string NomeCultivo { get; set; } = string.Empty;

        public string CategoriaCultivo { get; set; } = string.Empty;

        public string PorteCultivo { get; set; } = string.Empty;

        public string TempoColheita { get; set; } = string.Empty;

        public string VidaUtil { get; set; } = string.Empty;

        public string Intermitencia { get; set; } = string.Empty;
    }
}