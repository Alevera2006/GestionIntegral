using GestionIntegral.Application.DTOs.EstadosCompra;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class EstadosCompraController : BaseController
{
    private readonly IEstadoCompraService _estadoCompraService;

    public EstadosCompraController(IEstadoCompraService estadoCompraService)
    {
        _estadoCompraService = estadoCompraService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _estadoCompraService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _estadoCompraService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearEstadoCompraRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _estadoCompraService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarEstadoCompraRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _estadoCompraService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _estadoCompraService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
