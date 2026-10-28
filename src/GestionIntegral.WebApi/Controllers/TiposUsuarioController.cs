using GestionIntegral.Application.DTOs.TiposUsuario;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class TiposUsuarioController : BaseController
{
    private readonly ITipoUsuarioService _tipoUsuarioService;

    public TiposUsuarioController(ITipoUsuarioService tipoUsuarioService)
    {
        _tipoUsuarioService = tipoUsuarioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _tipoUsuarioService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _tipoUsuarioService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearTipoUsuarioRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _tipoUsuarioService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _tipoUsuarioService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
