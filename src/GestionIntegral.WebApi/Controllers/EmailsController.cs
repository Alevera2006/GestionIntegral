using GestionIntegral.Application.DTOs.Emails;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class EmailsController : BaseController
{
    private readonly IEmailService _emailService;

    public EmailsController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? tipoReferencia, [FromQuery] int? idReferencia, CancellationToken cancellationToken)
    {
        var response = await _emailService.ObtenerTodosAsync(tipoReferencia, idReferencia, cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _emailService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearEmailRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _emailService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarEmailRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _emailService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _emailService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
