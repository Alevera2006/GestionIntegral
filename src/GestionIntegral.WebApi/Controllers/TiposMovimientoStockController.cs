using GestionIntegral.Application.DTOs.TiposMovimientoStock;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class TiposMovimientoStockController : BaseController
{
    private readonly ITipoMovimientoStockService _tipoMovimientoStockService;

    public TiposMovimientoStockController(ITipoMovimientoStockService tipoMovimientoStockService)
    {
        _tipoMovimientoStockService = tipoMovimientoStockService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _tipoMovimientoStockService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _tipoMovimientoStockService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearTipoMovimientoStockRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _tipoMovimientoStockService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarTipoMovimientoStockRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _tipoMovimientoStockService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _tipoMovimientoStockService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
