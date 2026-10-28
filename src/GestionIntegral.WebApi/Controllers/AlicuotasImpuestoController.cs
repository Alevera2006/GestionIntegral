using GestionIntegral.Application.DTOs.AlicuotasImpuesto;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class AlicuotasImpuestoController : BaseController
{
    private readonly IAlicuotaImpuestoService _alicuotaImpuestoService;

    public AlicuotasImpuestoController(IAlicuotaImpuestoService alicuotaImpuestoService)
    {
        _alicuotaImpuestoService = alicuotaImpuestoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _alicuotaImpuestoService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("tipoImpuesto/{idTipoImpuesto:int}")]
    public async Task<IActionResult> GetByTipoImpuesto(int idTipoImpuesto, CancellationToken cancellationToken)
    {
        var response = await _alicuotaImpuestoService.ObtenerPorTipoImpuestoAsync(idTipoImpuesto, cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _alicuotaImpuestoService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearAlicuotaImpuestoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _alicuotaImpuestoService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarAlicuotaImpuestoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _alicuotaImpuestoService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _alicuotaImpuestoService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
