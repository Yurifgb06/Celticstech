namespace Celticstech.DTOs
{
    public class AssociacaoResponseDTO
    {
        public int IdAssociacao { get; set; }

        public string NomeAssociacao { get; set; } = string.Empty;

        public string SiglaAssociacao { get; set; } = string.Empty;

        public int IdRegiao { get; set; }

        public string? NomeRegiao { get; set; }

        public string Cnpj { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;
    }
}