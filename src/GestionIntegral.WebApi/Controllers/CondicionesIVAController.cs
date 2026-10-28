using GestionIntegral.Application.DTOs.CondicionesIVA;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class CondicionesIVAController : BaseController
{
    private readonly ICondicionIVAService _condicionIVAService;

    public CondicionesIVAController(ICondicionIVAService condicionIVAService)
    {
        _condicionIVAService = condicionIVAService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _condicionIVAService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _condicionIVAService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearCondicionIVARequestDto request, CancellationToken cancellationToken)
    {
        var response = await _condicionIVAService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarCondicionIVARequestDto request, CancellationToken cancellationToken)
    {
        var response = await _condicionIVAService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _condicionIVAService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
