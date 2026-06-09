namespace Celticstech.DTOs
{
    public class DashboardResumoResponseDTO
    {
        public int TotalRegioes { get; set; }

        public int TotalAssociacoes { get; set; }

        public int TotalCultivos { get; set; }

        public int TotalAgricultores { get; set; }

        public int TotalRecomendacoes { get; set; }

        public string IntegracaoOpenMeteo { get; set; } = "Ativa";

        public string StatusSistema { get; set; } = "Operacional";
    }
}