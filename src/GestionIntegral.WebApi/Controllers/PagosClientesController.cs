using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.CuentasCorrientes;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PagosClientesController : ControllerBase
{
    private readonly ICuentaCorrienteService _cuentaCorrienteService;

    public PagosClientesController(ICuentaCorrienteService cuentaCorrienteService)
    {
        _cuentaCorrienteService = cuentaCorrienteService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PagoClienteRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _cuentaCorrienteService.RegistrarPagoAsync(request, cancellationToken);
        return Ok(response);
    }
}
