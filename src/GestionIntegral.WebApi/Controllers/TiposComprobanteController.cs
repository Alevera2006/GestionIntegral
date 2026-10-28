using GestionIntegral.Application.DTOs.TiposComprobante;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class TiposComprobanteController : BaseController
{
    private readonly ITipoComprobanteService _tipoComprobanteService;

    public TiposComprobanteController(ITipoComprobanteService tipoComprobanteService)
    {
        _tipoComprobanteService = tipoComprobanteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _tipoComprobanteService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _tipoComprobanteService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearTipoComprobanteRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _tipoComprobanteService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarTipoComprobanteRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _tipoComprobanteService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _tipoComprobanteService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
