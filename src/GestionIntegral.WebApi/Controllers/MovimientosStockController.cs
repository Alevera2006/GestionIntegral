using GestionIntegral.Application.DTOs.MovimientosStock;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
public class MovimientosStockController : BaseController
{
    private readonly IMovimientoStockService _movimientoStockService;
    private readonly IMovimientoStockDetalleService _detalleService;

    public MovimientosStockController(IMovimientoStockService movimientoStockService, IMovimientoStockDetalleService detalleService)
    {
        _movimientoStockService = movimientoStockService;
        _detalleService = detalleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _movimientoStockService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await _movimientoStockService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearMovimientoStockRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _movimientoStockService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarMovimientoStockRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _movimientoStockService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var response = await _movimientoStockService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpGet("{idMovimiento:long}/detalles")]
    public async Task<IActionResult> GetDetalles(long idMovimiento, CancellationToken cancellationToken)
    {
        var response = await _detalleService.ObtenerPorMovimientoAsync(idMovimiento, cancellationToken);
        return Result(response);
    }

    [HttpPost("detalles")]
    public async Task<IActionResult> CreateDetalle([FromBody] CrearMovimientoStockDetalleRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _detalleService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("detalles/{id:long}")]
    public async Task<IActionResult> DeleteDetalle(long id, CancellationToken cancellationToken)
    {
        var response = await _detalleService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
