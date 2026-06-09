using System.ComponentModel.DataAnnotations;

namespace Celticstech.DTOs
{
    public class AgricultorDTO
    {
        [Required(ErrorMessage = "O nome do agricultor e obrigatorio.")]
        [MaxLength(80, ErrorMessage = "O nome do agricultor deve ter no maximo 80 caracteres.")]
        public string NomeAgricultor { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF do agricultor e obrigatorio.")]
        [RegularExpression(@"^\d{3}\.?\d{3}\.?\d{3}-?\d{2}$", ErrorMessage = "CPF invalido. Use 98765432100 ou 987.654.321-00.")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone do agricultor e obrigatorio.")]
        [RegularExpression(@"^\(?\d{2}\)?\s?\d{4,5}-?\d{4}$", ErrorMessage = "Telefone invalido. Use 71991234567 ou (71) 99123-4567.")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "A associacao e obrigatoria.")]
        public int IdAssociacao { get; set; }

        [Required(ErrorMessage = "O cultivo e obrigatorio.")]
        public int IdCultivo { get; set; }
    }
}