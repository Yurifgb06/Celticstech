namespace Celticstech.DTOs
{
    public class OpenMeteoResponseDTO
    {
        public double Temperatura { get; set; }

        public double Umidade { get; set; }

        public double VelocidadeVento { get; set; }

        public double Chuva { get; set; }

        public int ScoreRisco { get; set; }

        public string NivelRisco { get; set; } = string.Empty;
    }
}
