using GestionIntegral.Application.DTOs.TiposImpuesto;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class TiposImpuestoController : BaseController
{
    private readonly ITipoImpuestoService _tipoImpuestoService;

    public TiposImpuestoController(ITipoImpuestoService tipoImpuestoService)
    {
        _tipoImpuestoService = tipoImpuestoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _tipoImpuestoService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _tipoImpuestoService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearTipoImpuestoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _tipoImpuestoService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarTipoImpuestoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _tipoImpuestoService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _tipoImpuestoService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
