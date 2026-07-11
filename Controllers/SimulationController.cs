using ExamenAPI.Services;
using ExamenUnidad2Paradigmas.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ExamenAPI.Controllers;

[Route("api/simulations")]
[ApiController]
public class SimulationController : ControllerBase
{
    private readonly ISimulationService _simulationService;

    public SimulationController(ISimulationService simulationService)
    {
        _simulationService = simulationService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var response = await _simulationService.GetAllSimulationsAsync();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var response = await _simulationService.GetSimulationByIdAsync(id);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] SimulationCreateDto dto)
    {
        var response = await _simulationService.CreateSimulationAsync(dto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] SimulationCreateDto dto)
    {

        var response = await _simulationService.UpdateSimulationAsync(id, dto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var response = await _simulationService.DeleteSimulationAsync(id);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id}/monthly")]
    public async Task<ActionResult> GetMonthlyProjection(int id)
    {
        var response = await _simulationService.GetMonthlyProjectionsAsync(id);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id}/annual")]
    public async Task<ActionResult> GetAnnualProjection(int id)
    {
        var response = await _simulationService.GetAnnualProjectionsAsync(id);
        return StatusCode(response.StatusCode, response);
    }
}