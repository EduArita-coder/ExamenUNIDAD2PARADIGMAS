using System.ComponentModel.DataAnnotations;

namespace ExamenUnidad2Paradigmas.Dtos
{
    public class SimulationCreateDto
    {
        [Required(ErrorMessage = "El monto inicial es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto inicial debe ser mayor a 0")]
        public decimal MontoInicial { get; set; }

        [Required(ErrorMessage = "La tasa de interés anual es requerida")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "La tasa de interés anual debe ser mayor a 0")]
        public decimal TasaInteresAnual { get; set; }

        [Required(ErrorMessage = "El plazo en años es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El plazo en años debe ser mayor a 0")]
        public int PlazoAnios { get; set; }
    }
}