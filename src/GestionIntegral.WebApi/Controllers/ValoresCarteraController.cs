using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.ValoresCartera;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ValoresCarteraController : ControllerBase
{
    private readonly IValorCarteraService _valorCarteraService;

    public ValoresCarteraController(IValorCarteraService valorCarteraService)
    {
        _valorCarteraService = valorCarteraService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _valorCarteraService.ObtenerTodosAsync(cancellationToken);
        return Ok(response);
    }
}
