namespace Celticstech.DTOs
{
    public class RecomendacaoResponseDTO
    {
        public int IdRecomendacao { get; set; }

        public DateTime DataRecAsc { get; set; }

        public int IdAssociacao { get; set; }

        public string NomeAssociacao { get; set; } = string.Empty;

        public int IdCultivo { get; set; }

        public string NomeCultivo { get; set; } = string.Empty;

        public string CategoriaCultivo { get; set; } = string.Empty;

        public string PorteCultivo { get; set; } = string.Empty;

        public string Orientacao { get; set; } = string.Empty;

        public string TipoRecomendacao { get; set; } = string.Empty;
    }
}