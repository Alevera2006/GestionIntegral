using GestionIntegral.Application.DTOs.MotivosAnulacion;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class MotivosAnulacionController : BaseController
{
    private readonly IMotivoAnulacionService _motivoAnulacionService;

    public MotivosAnulacionController(IMotivoAnulacionService motivoAnulacionService)
    {
        _motivoAnulacionService = motivoAnulacionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _motivoAnulacionService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _motivoAnulacionService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearMotivoAnulacionRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _motivoAnulacionService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarMotivoAnulacionRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _motivoAnulacionService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _motivoAnulacionService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
