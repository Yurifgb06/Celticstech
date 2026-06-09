namespace Celticstech.DTOs
{
    public class AgricultorResponseDTO
    {
        public int IdAgricultor { get; set; }

        public string NomeAgricultor { get; set; } = string.Empty;

        public string Cpf { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

        public int IdAssociacao { get; set; }

        public string NomeAssociacao { get; set; } = string.Empty;

        public int IdCultivo { get; set; }

        public string NomeCultivo { get; set; } = string.Empty;
    }
}