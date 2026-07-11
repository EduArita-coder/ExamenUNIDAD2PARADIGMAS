namespace ExamenUnidad2Paradigmas.Dtos
{
    public class SimulationDto
    {
        public int Id { get; set; }
        public double MontoInicial { get; set; }     // Capital inicial
        public double TasaInteresAnual { get; set; } // Tasa anual en decimal (ej. 0.05)
        public int PlazoAnios { get; set; }
        public double MontoFinal { get; set; }
        public double InteresTotal { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}