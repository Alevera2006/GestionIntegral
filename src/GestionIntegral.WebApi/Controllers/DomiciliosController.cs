using GestionIntegral.Application.DTOs.Domicilios;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class DomiciliosController : BaseController
{
    private readonly IDomicilioService _domicilioService;

    public DomiciliosController(IDomicilioService domicilioService)
    {
        _domicilioService = domicilioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? tipoReferencia, [FromQuery] int? idReferencia, CancellationToken cancellationToken)
    {
        var response = await _domicilioService.ObtenerTodosAsync(tipoReferencia, idReferencia, cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _domicilioService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearDomicilioRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _domicilioService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarDomicilioRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _domicilioService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _domicilioService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
