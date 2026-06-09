namespace Celticstech.DTOs
{
    public class ClimaRegiaoResponseDTO
    {
        public int IdRegiao { get; set; }

        public string Regiao { get; set; } = string.Empty;

        public string Uf { get; set; } = string.Empty;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Temperatura { get; set; }

        public double Umidade { get; set; }

        public double VelocidadeVento { get; set; }

        public double Chuva { get; set; }

        public int ScoreRisco { get; set; }

        public string NivelRisco { get; set; } = string.Empty;

        public string FonteDados { get; set; } = "Open-Meteo";
    }
}
