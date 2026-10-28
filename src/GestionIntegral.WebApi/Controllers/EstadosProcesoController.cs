using GestionIntegral.Application.DTOs.EstadosProceso;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class EstadosProcesoController : BaseController
{
    private readonly IEstadoProcesoService _estadoProcesoService;

    public EstadosProcesoController(IEstadoProcesoService estadoProcesoService)
    {
        _estadoProcesoService = estadoProcesoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _estadoProcesoService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _estadoProcesoService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearEstadoProcesoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _estadoProcesoService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarEstadoProcesoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _estadoProcesoService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _estadoProcesoService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
