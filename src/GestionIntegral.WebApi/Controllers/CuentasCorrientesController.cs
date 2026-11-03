using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.CuentasCorrientes;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CuentasCorrientesController : ControllerBase
{
    private readonly ICuentaCorrienteService _cuentaCorrienteService;

    public CuentasCorrientesController(ICuentaCorrienteService cuentaCorrienteService)
    {
        _cuentaCorrienteService = cuentaCorrienteService;
    }

    [HttpGet("clientes")]
    public async Task<IActionResult> GetSaldosClientes(CancellationToken cancellationToken)
    {
        var response = await _cuentaCorrienteService.ObtenerSaldosClientesAsync(cancellationToken);
        return Ok(response);
    }

    [HttpGet("{idCliente:int}/movimientos")]
    public async Task<IActionResult> GetMovimientos(int idCliente, CancellationToken cancellationToken)
    {
        var response = await _cuentaCorrienteService.ObtenerMovimientosAsync(idCliente, cancellationToken);
        return Ok(response);
    }
}
