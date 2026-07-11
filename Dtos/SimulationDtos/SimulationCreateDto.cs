using System.ComponentModel.DataAnnotations;

namespace ExamenUnidad2Paradigmas.Dtos
{
    public class SimulationCreateDto
    {
        [Required(ErrorMessage = "El {0} es requerido")]
        [StringLength(13, ErrorMessage = "El {0} debe tener {1} dígitos.", MinimumLength = 13)]
        public string MontonInicial { get; set; }
        [Required(ErrorMessage = "El {0} es requerido")]
        [StringLength(13, ErrorMessage = "El {0} debe tener {1} dígitos.", MinimumLength = 13)]
        public string TasaInteresAnual { get; set; }
        [Required(ErrorMessage = "El {0} es requerido")]
        [StringLength(13, ErrorMessage = "El {0} debe tener {1} dígitos.", MinimumLength = 13)]
        public int PlazoAnios { get; set; }
    }
}