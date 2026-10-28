using GestionIntegral.Application.DTOs.MediosPago;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class MediosPagoController : BaseController
{
    private readonly IMedioPagoService _medioPagoService;

    public MediosPagoController(IMedioPagoService medioPagoService)
    {
        _medioPagoService = medioPagoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _medioPagoService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _medioPagoService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearMedioPagoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _medioPagoService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarMedioPagoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _medioPagoService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _medioPagoService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
