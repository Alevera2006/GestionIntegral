using GestionIntegral.Application.DTOs.Telefonos;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class TelefonosController : BaseController
{
    private readonly ITelefonoService _telefonoService;

    public TelefonosController(ITelefonoService telefonoService)
    {
        _telefonoService = telefonoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? tipoReferencia, [FromQuery] int? idReferencia, CancellationToken cancellationToken)
    {
        var response = await _telefonoService.ObtenerTodosAsync(tipoReferencia, idReferencia, cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _telefonoService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearTelefonoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _telefonoService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarTelefonoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _telefonoService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _telefonoService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
