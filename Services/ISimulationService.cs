#nullable enable

using ExamenUnidad2Paradigmas.Dtos;
using ExamenUnidad2Paradigmas.Dtos.Common;

namespace ExamenAPI.Services;

public interface ISimulationService
{
    Task<ResponseDto<List<SimulationDto>>> GetAllSimulationsAsync();
    Task<ResponseDto<SimulationDto>> GetSimulationByIdAsync(int id);
    Task<ResponseDto<SimulationActionResponseDto>> CreateSimulationAsync(SimulationCreateDto simulation);
    Task<ResponseDto<SimulationActionResponseDto>> UpdateSimulationAsync(int id, SimulationCreateDto simulation);
    Task<ResponseDto<bool>> DeleteSimulationAsync(int id);
    Task<ResponseDto<List<MonthlyProjectionDto>>> GetMonthlyProjectionsAsync(int id);
    Task<ResponseDto<List<AnnualProjectionDto>>> GetAnnualProjectionsAsync(int id);
}
