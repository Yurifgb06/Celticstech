namespace Celticstech.DTOs
{
    public class HistoricoClimaticoResponseDTO
    {
        public int IdRecomendacao { get; set; }

        public DateTime Data { get; set; }

        public string Associacao { get; set; } = string.Empty;

        public string Cultivo { get; set; } = string.Empty;

        public string NivelRisco { get; set; } = string.Empty;

        public int ScoreRisco { get; set; }

        public double Temperatura { get; set; }

        public double Umidade { get; set; }

        public double Chuva { get; set; }

        public double VelocidadeVento { get; set; }

        public string Orientacao { get; set; } = string.Empty;

        public string FonteDados { get; set; } = string.Empty;
    }
}
