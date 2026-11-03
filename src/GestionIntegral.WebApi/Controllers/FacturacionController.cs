using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Facturacion;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FacturacionController : ControllerBase
{
    private readonly IFacturaService _facturaService;

    public FacturacionController(IFacturaService facturaService)
    {
        _facturaService = facturaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _facturaService.ObtenerTodosAsync(cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FacturaRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _facturaService.CrearAsync(request, cancellationToken);
        return Ok(response);
    }
}
