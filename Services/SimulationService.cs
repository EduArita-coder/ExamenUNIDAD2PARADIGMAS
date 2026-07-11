#nullable enable

using ExamenAPI.Database;
using ExamenAPI.Entities;
using ExamenUnidad2Paradigmas.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ExamenAPI.Services;

public class SimulationService : ISimulationService
{
    private readonly SimulationDbContext _context;

    public SimulationService(SimulationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SimulationDto>> GetAllSimulationsAsync()
    {
        var entities = await _context.Simulations.ToListAsync();
        return entities.Select(ToDto).ToList();
    }

    public async Task<SimulationDto?> GetSimulationByIdAsync(int id)
    {
        var entity = await _context.Simulations.FindAsync(id);
        return entity is null ? null : ToDto(entity);
    }

    public async Task<SimulationDto> CreateSimulationAsync(SimulationCreateDto simulation)
    {
        var entity = CreateEntity(simulation);
        entity.BalanceFinal = CalculateBalanceFinal(entity.DepositoInicial, entity.TasaInteresAnual, entity.PlazoEnAños);
        entity.InteresTotal = entity.BalanceFinal - entity.DepositoInicial;

        _context.Simulations.Add(entity);
        await _context.SaveChangesAsync();

        return ToDto(entity);
    }

    public async Task<SimulationDto?> UpdateSimulationAsync(int id, SimulationCreateDto simulation)
    {
        var existing = await _context.Simulations.FindAsync(id);
        if (existing is null)
        {
            return null;
        }

        existing.DepositoInicial = ParseDecimal(nameof(simulation.MontonInicial), simulation.MontonInicial);
        existing.TasaInteresAnual = ParseDecimal(nameof(simulation.TasaInteresAnual), simulation.TasaInteresAnual);
        existing.PlazoEnAños = simulation.PlazoAnios;
        existing.BalanceFinal = CalculateBalanceFinal(existing.DepositoInicial, existing.TasaInteresAnual, existing.PlazoEnAños);
        existing.InteresTotal = existing.BalanceFinal - existing.DepositoInicial;

        await _context.SaveChangesAsync();
        return ToDto(existing);
    }

    public async Task<bool> DeleteSimulationAsync(int id)
    {
        var existing = await _context.Simulations.FindAsync(id);
        if (existing is null)
        {
            return false;
        }

        _context.Simulations.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<MonthlyProjectionDto>?> GetMonthlyProjectionsAsync(int id)
    {
        var simulation = await _context.Simulations.FindAsync(id);
        return simulation is null ? null : CalculateMonthlyProjection(simulation.DepositoInicial, simulation.TasaInteresAnual, simulation.PlazoEnAños);
    }

    public async Task<IEnumerable<AnnualProjectionDto>?> GetAnnualProjectionsAsync(int id)
    {
        var simulation = await _context.Simulations.FindAsync(id);
        return simulation is null ? null : CalculateAnnualProjection(simulation.DepositoInicial, simulation.TasaInteresAnual, simulation.PlazoEnAños);
    }

    private static SimulationDto ToDto(SimulationEntity entity)
    {
        return new SimulationDto
        {
            Id = entity.Id,
            MontoInicial = (double)entity.DepositoInicial,
            TasaInteresAnual = (double)entity.TasaInteresAnual,
            PlazoAnios = entity.PlazoEnAños,
            MontoFinal = (double)entity.BalanceFinal,
            InteresTotal = (double)entity.InteresTotal,
            FechaCreacion = DateTime.UtcNow
        };
    }

    private static SimulationEntity CreateEntity(SimulationCreateDto simulation)
    {
        return new SimulationEntity
        {
            DepositoInicial = ParseDecimal(nameof(simulation.MontonInicial), simulation.MontonInicial),
            TasaInteresAnual = ParseDecimal(nameof(simulation.TasaInteresAnual), simulation.TasaInteresAnual),
            PlazoEnAños = simulation.PlazoAnios
        };
    }

    private static decimal ParseDecimal(string fieldName, string value)
    {
        if (!decimal.TryParse(value, out var result))
        {
            throw new ArgumentException($"El campo '{fieldName}' debe ser un número válido.");
        }

        return result;
    }

    private static decimal CalculateBalanceFinal(decimal capital, decimal annualRate, int years)
    {
        var monthlyRate = annualRate / 12m;
        var totalMonths = years * 12;
        return capital * DecimalPow(1m + monthlyRate, totalMonths);
    }

    private static List<MonthlyProjectionDto> CalculateMonthlyProjection(decimal capital, decimal annualRate, int years)
    {
        var monthlyRate = annualRate / 12m;
        var totalMonths = years * 12;
        var balance = capital;
        var projections = new List<MonthlyProjectionDto>(totalMonths);

        for (var month = 1; month <= totalMonths; month++)
        {
            balance *= 1m + monthlyRate;
            projections.Add(new MonthlyProjectionDto
            {
                Mes = month,
                Balance = (double)balance,
                Interes = (double)(balance - capital)
            });
        }

        return projections;
    }

    private static List<AnnualProjectionDto> CalculateAnnualProjection(decimal capital, decimal annualRate, int years)
    {
        var monthlyRate = annualRate / 12m;
        var balance = capital;
        var projections = new List<AnnualProjectionDto>(years);

        for (var year = 1; year <= years; year++)
        {
            for (var month = 1; month <= 12; month++)
            {
                balance *= 1m + monthlyRate;
            }

            projections.Add(new AnnualProjectionDto
            {
                Anio = year,
                Balance = (double)balance,
                Interes = (double)(balance - capital)
            });
        }

        return projections;
    }

    private static decimal DecimalPow(decimal value, int exponent)
    {
        var result = 1m;
        for (var i = 0; i < exponent; i++)
        {
            result *= value;
        }

        return result;
    }
}
