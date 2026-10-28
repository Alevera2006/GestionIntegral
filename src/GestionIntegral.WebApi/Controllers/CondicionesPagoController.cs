using GestionIntegral.Application.DTOs.CondicionesPago;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class CondicionesPagoController : BaseController
{
    private readonly ICondicionPagoService _condicionPagoService;

    public CondicionesPagoController(ICondicionPagoService condicionPagoService)
    {
        _condicionPagoService = condicionPagoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _condicionPagoService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _condicionPagoService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearCondicionPagoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _condicionPagoService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarCondicionPagoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _condicionPagoService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _condicionPagoService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
