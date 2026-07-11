#nullable enable

using ExamenAPI.Database;
using ExamenAPI.Entities;
using ExamenUnidad2Paradigmas.Dtos;
using ExamenUnidad2Paradigmas.Dtos.Common;
using Microsoft.EntityFrameworkCore;
using PersonsApp.Constants;

namespace ExamenAPI.Services;

public class SimulationService : ISimulationService
{
    private readonly SimulationDbContext _context;

    public SimulationService(SimulationDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseDto<List<SimulationDto>>> GetAllSimulationsAsync()
    {
        var entities = await _context.Simulations
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        return new ResponseDto<List<SimulationDto>>
        {
            StatusCode = HttpStatusCode.OK,
            Status = true,
            Message = HttpMessageResponse.REGISTERS_FOUND,
            Data = entities.Select(ToDto).ToList()
        };
    }

    public async Task<ResponseDto<SimulationDto>> GetSimulationByIdAsync(int id)
    {
        var entity = await _context.Simulations.FindAsync(id);
        if (entity is null)
        {
            return new ResponseDto<SimulationDto>
            {
                StatusCode = HttpStatusCode.NOT_FOUND,
                Status = false,
                Message = HttpMessageResponse.REGISTER_NOT_FOUND
            };
        }

        return new ResponseDto<SimulationDto>
        {
            StatusCode = HttpStatusCode.OK,
            Status = true,
            Message = HttpMessageResponse.REGISTER_FOUND,
            Data = ToDto(entity)
        };
    }

    public async Task<ResponseDto<SimulationActionResponseDto>> CreateSimulationAsync(SimulationCreateDto simulation)
    {
        var validationMessage = ValidateSimulation(simulation);
        if (validationMessage is not null)
        {
            return new ResponseDto<SimulationActionResponseDto>
            {
                StatusCode = HttpStatusCode.BAD_REQUEST,
                Status = false,
                Message = validationMessage
            };
        }

        var entity = new SimulationEntity
        {
            DepositoInicial = simulation.MontoInicial,
            TasaInteresAnual = simulation.TasaInteresAnual,
            PlazoEnAños = simulation.PlazoAnios,
            FechaCreacion = DateTime.UtcNow
        };

        entity.BalanceFinal = CalcularBalanceFinal(entity.DepositoInicial, entity.TasaInteresAnual, entity.PlazoEnAños);
        entity.InteresTotal = entity.BalanceFinal - entity.DepositoInicial;

        _context.Simulations.Add(entity);
        await _context.SaveChangesAsync();

        return new ResponseDto<SimulationActionResponseDto>
        {
            StatusCode = HttpStatusCode.CREATED,
            Status = true,
            Message = HttpMessageResponse.REGISTER_CREATED,
            Data = new SimulationActionResponseDto { Id = entity.Id }
        };
    }

    public async Task<ResponseDto<SimulationActionResponseDto>> UpdateSimulationAsync(int id, SimulationCreateDto simulation)
    {
        var validationMessage = ValidateSimulation(simulation);
        if (validationMessage is not null)
        {
            return new ResponseDto<SimulationActionResponseDto>
            {
                StatusCode = HttpStatusCode.BAD_REQUEST,
                Status = false,
                Message = validationMessage
            };
        }

        var existing = await _context.Simulations.FindAsync(id);
        if (existing is null)
        {
            return new ResponseDto<SimulationActionResponseDto>
            {
                StatusCode = HttpStatusCode.NOT_FOUND,
                Status = false,
                Message = HttpMessageResponse.REGISTER_NOT_FOUND
            };
        }

        existing.DepositoInicial = simulation.MontoInicial;
        existing.TasaInteresAnual = simulation.TasaInteresAnual;
        existing.PlazoEnAños = simulation.PlazoAnios;
        existing.BalanceFinal = CalcularBalanceFinal(existing.DepositoInicial, existing.TasaInteresAnual, existing.PlazoEnAños);
        existing.InteresTotal = existing.BalanceFinal - existing.DepositoInicial;

        await _context.SaveChangesAsync();

        return new ResponseDto<SimulationActionResponseDto>
        {
            StatusCode = HttpStatusCode.OK,
            Status = true,
            Message = HttpMessageResponse.REGISTER_UPDATED,
            Data = new SimulationActionResponseDto { Id = existing.Id }
        };
    }

    public async Task<ResponseDto<bool>> DeleteSimulationAsync(int id)
    {
        var existing = await _context.Simulations.FindAsync(id);
        if (existing is null)
        {
            return new ResponseDto<bool>
            {
                StatusCode = HttpStatusCode.NOT_FOUND,
                Status = false,
                Message = HttpMessageResponse.REGISTER_NOT_FOUND
            };
        }

        _context.Simulations.Remove(existing);
        await _context.SaveChangesAsync();

        return new ResponseDto<bool>
        {
            StatusCode = HttpStatusCode.OK,
            Status = true,
            Message = HttpMessageResponse.REGISTER_DELETED,
            Data = true
        };
    }

    public async Task<ResponseDto<List<MonthlyProjectionDto>>> GetMonthlyProjectionsAsync(int id)
    {
        var simulation = await _context.Simulations.FindAsync(id);
        if (simulation is null)
        {
            return new ResponseDto<List<MonthlyProjectionDto>>
            {
                StatusCode = HttpStatusCode.NOT_FOUND,
                Status = false,
                Message = HttpMessageResponse.REGISTER_NOT_FOUND
            };
        }

        return new ResponseDto<List<MonthlyProjectionDto>>
        {
            StatusCode = HttpStatusCode.OK,
            Status = true,
            Message = HttpMessageResponse.REGISTER_FOUND,
            Data = CalcularProyeccionMensual(simulation.DepositoInicial, simulation.TasaInteresAnual, simulation.PlazoEnAños)
        };
    }

    public async Task<ResponseDto<List<AnnualProjectionDto>>> GetAnnualProjectionsAsync(int id)
    {
        var simulation = await _context.Simulations.FindAsync(id);
        if (simulation is null)
        {
            return new ResponseDto<List<AnnualProjectionDto>>
            {
                StatusCode = HttpStatusCode.NOT_FOUND,
                Status = false,
                Message = HttpMessageResponse.REGISTER_NOT_FOUND
            };
        }

        return new ResponseDto<List<AnnualProjectionDto>>
        {
            StatusCode = HttpStatusCode.OK,
            Status = true,
            Message = HttpMessageResponse.REGISTER_FOUND,
            Data = CalculateAnnualProjection(simulation.DepositoInicial, simulation.TasaInteresAnual, simulation.PlazoEnAños)
        };
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
            FechaCreacion = entity.FechaCreacion
        };
    }

    private static string? ValidateSimulation(SimulationCreateDto simulation)
    {
        if (simulation.MontoInicial <= 0)
        {
            return "El monto inicial debe ser mayor que cero.";
        }

        if (simulation.TasaInteresAnual <= 0)
        {
            return "La tasa de interés anual debe ser mayor que cero.";
        }

        if (simulation.PlazoAnios <= 0)
        {
            return "El plazo en años debe ser mayor que cero.";
        }

        return null;
    }

    private static decimal CalcularBalanceFinal(decimal capital, decimal annualRate, int years)
    {
        var monthlyRate = annualRate / 12m;
        var totalMonths = years * 12;
        return capital * (decimal)Math.Pow((double)(1m + monthlyRate), totalMonths);
    }

    private static List<MonthlyProjectionDto> CalcularProyeccionMensual(decimal capital, decimal annualRate, int years)
    {
        var monthlyRate = annualRate / 12m;
        var totalMonths = years * 12;
        decimal balance = capital;
        var projections = new List<MonthlyProjectionDto>(totalMonths);

        for (var month = 1; month <= totalMonths; month++)
        {
            var previousBalance = balance;
            balance *= 1m + monthlyRate;
            var interest = balance - previousBalance;

            projections.Add(new MonthlyProjectionDto
            {
                Mes = month,
                Balance = (double)balance,
                Interes = (double)interest
            });
        }

        return projections;
    }

    private static List<AnnualProjectionDto> CalculateAnnualProjection(decimal capital, decimal annualRate, int years)
    {
        var monthlyRate = annualRate / 12m;
        decimal balance = capital;
        var projections = new List<AnnualProjectionDto>(years);

        for (var year = 1; year <= years; year++)
        {
            var startBalance = balance;

            for (var month = 1; month <= 12; month++)
            {
                balance *= 1m + monthlyRate;
            }

            var interest = balance - startBalance;

            projections.Add(new AnnualProjectionDto
            {
                Anio = year,
                Balance = (double)balance,
                Interes = (double)interest
            });
        }

        return projections;
    }
}
