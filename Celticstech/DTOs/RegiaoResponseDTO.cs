namespace Celticstech.DTOs
{
    public class RegiaoResponseDTO
    {
        public int IdRegiao { get; set; }

        public string NomeRegiao { get; set; } = string.Empty;

        public string UfRegiao { get; set; } = string.Empty;

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}