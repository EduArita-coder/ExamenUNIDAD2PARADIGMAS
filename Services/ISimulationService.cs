#nullable enable

using ExamenUnidad2Paradigmas.Dtos;

namespace ExamenAPI.Services;

public interface ISimulationService
{
    Task<IEnumerable<SimulationDto>> GetAllSimulationsAsync();
    Task<SimulationDto?> GetSimulationByIdAsync(int id);
    Task<SimulationDto> CreateSimulationAsync(SimulationCreateDto simulation);
    Task<SimulationDto?> UpdateSimulationAsync(int id, SimulationCreateDto simulation);
    Task<bool> DeleteSimulationAsync(int id);
    Task<IEnumerable<MonthlyProjectionDto>?> GetMonthlyProjectionsAsync(int id);
    Task<IEnumerable<AnnualProjectionDto>?> GetAnnualProjectionsAsync(int id);
}
