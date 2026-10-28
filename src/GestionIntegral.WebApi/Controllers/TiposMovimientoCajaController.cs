using GestionIntegral.Application.DTOs.TiposMovimientoCaja;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class TiposMovimientoCajaController : BaseController
{
    private readonly ITipoMovimientoCajaService _tipoMovimientoCajaService;

    public TiposMovimientoCajaController(ITipoMovimientoCajaService tipoMovimientoCajaService)
    {
        _tipoMovimientoCajaService = tipoMovimientoCajaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _tipoMovimientoCajaService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _tipoMovimientoCajaService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearTipoMovimientoCajaRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _tipoMovimientoCajaService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarTipoMovimientoCajaRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _tipoMovimientoCajaService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _tipoMovimientoCajaService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
