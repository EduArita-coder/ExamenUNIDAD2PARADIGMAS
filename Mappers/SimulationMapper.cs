using ExamenUnidad2Paradigmas.Dtos;
using ExamenAPI.Entities;

namespace ExamenAPI.Mappers;

public static class SimulationMapper
{
    public static SimulationEntity ToEntity(SimulationCreateDto dto)
    {
        return new SimulationEntity
        {
            DepositoInicial = decimal.Parse(dto.MontonInicial),
            TasaInteresAnual = decimal.Parse(dto.TasaInteresAnual),
            PlazoEnAños = dto.PlazoAnios
        };
    }

    public static SimulationDto ToDto(SimulationEntity entity)
    {
        return new SimulationDto
        {
            Id = entity.Id,
            MontoInicial = (double)entity.DepositoInicial,
            TasaInteresAnual = (double)entity.TasaInteresAnual,
            PlazoAnios = entity.PlazoEnAños,
            MontoFinal = (double)entity.BalanceFinal,
            InteresTotal = (double)entity.InteresTotal
        };
    }
}