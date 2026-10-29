using System.ComponentModel.DataAnnotations;
using GestionIntegral.Application.DTOs.Provincias;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class ProvinciasController : BaseController
{
    private readonly IProvinciaService _provinciaService;

    public ProvinciasController(IProvinciaService provinciaService)
    {
        _provinciaService = provinciaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _provinciaService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _provinciaService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearProvinciaRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _provinciaService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarProvinciaRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _provinciaService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([Range(1, int.MaxValue)] int id, CancellationToken cancellationToken)
    {
        var response = await _provinciaService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
