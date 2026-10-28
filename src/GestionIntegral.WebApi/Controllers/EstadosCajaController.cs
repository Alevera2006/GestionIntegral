using GestionIntegral.Application.DTOs.EstadosCaja;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class EstadosCajaController : BaseController
{
    private readonly IEstadoCajaService _estadoCajaService;

    public EstadosCajaController(IEstadoCajaService estadoCajaService)
    {
        _estadoCajaService = estadoCajaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _estadoCajaService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _estadoCajaService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearEstadoCajaRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _estadoCajaService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarEstadoCajaRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _estadoCajaService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _estadoCajaService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
