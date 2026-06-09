namespace Celticstech.DTOs
{
    public class DiagnosticoResponseDTO
    {
        public string Regiao { get; set; } = string.Empty;

        public string Cultivo { get; set; } = string.Empty;

        public int? IdCultivo { get; set; }

        public double Temperatura { get; set; }

        public double Umidade { get; set; }

        public double Chuva { get; set; }

        public double VelocidadeVento { get; set; }

        public int ScoreRisco { get; set; }

        public string NivelRisco { get; set; } = string.Empty;

        public string Recomendacao { get; set; } = string.Empty;

        public string ResumoRisco { get; set; } = string.Empty;

        public string MotivoRisco { get; set; } = string.Empty;

        public List<string> AcoesRecomendadas { get; set; } = [];

        public string Prioridade { get; set; } = string.Empty;

        public string PrazoSugerido { get; set; } = string.Empty;

        public string ObservacaoTecnica { get; set; } = string.Empty;

        public string FonteDados { get; set; } = "Open-Meteo API";
    }
}