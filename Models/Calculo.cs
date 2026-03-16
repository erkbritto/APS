using System.ComponentModel.DataAnnotations;

namespace APS.Models
{
    public class Calculo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O tipo de cálculo é obrigatório")]
        [StringLength(50)]
        public string Tipo { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "A entrada deve ser um número positivo")]
        public double Entrada { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O resultado deve ser um número positivo")]
        public double Resultado { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{Tipo} - Entrada: {Entrada:F2}, Resultado: {Resultado:F2} ({Timestamp:g})";
        }
    }
}
