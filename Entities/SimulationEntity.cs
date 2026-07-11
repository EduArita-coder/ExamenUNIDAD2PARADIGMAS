using System.ComponentModel.DataAnnotations;

namespace ExamenAPI.Entities;

public class SimulationEntity
{
    public int Id { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El {0} debe ser mayor a 0")]
    public decimal DepositoInicial { get; set; }

    [Required]
    [Range(0.0001, double.MaxValue, ErrorMessage = "El {0} debe ser mayor a 0")]
    public decimal TasaInteresAnual { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "El {0} debe ser mayor a 0")]
    public int PlazoEnAños { get; set; }

    public decimal BalanceFinal { get; set; }

    public decimal InteresTotal { get; set; }

    public DateTime FechaCreacion { get; set; }
}
