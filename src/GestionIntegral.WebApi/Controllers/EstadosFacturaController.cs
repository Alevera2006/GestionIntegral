using GestionIntegral.Application.DTOs.EstadosFactura;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class EstadosFacturaController : BaseController
{
    private readonly IEstadoFacturaService _estadoFacturaService;

    public EstadosFacturaController(IEstadoFacturaService estadoFacturaService)
    {
        _estadoFacturaService = estadoFacturaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _estadoFacturaService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _estadoFacturaService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearEstadoFacturaRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _estadoFacturaService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarEstadoFacturaRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _estadoFacturaService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _estadoFacturaService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
