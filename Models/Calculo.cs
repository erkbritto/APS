namespace APS.Models
{
    public class Calculo
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public double Entrada { get; set; }
        public double Resultado { get; set; }
        public DateTime Timestamp { get; set; }
    }
}