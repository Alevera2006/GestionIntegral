using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.PagosProveedores;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PagosProveedoresController : ControllerBase
{
    private readonly IPagoProveedorService _pagoProveedorService;

    public PagosProveedoresController(IPagoProveedorService pagoProveedorService)
    {
        _pagoProveedorService = pagoProveedorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _pagoProveedorService.ObtenerTodosAsync(cancellationToken);
        return Ok(response);
    }

    [HttpGet("proveedor/{idProveedor:int}")]
    public async Task<IActionResult> GetByProveedor(int idProveedor, CancellationToken cancellationToken)
    {
        var response = await _pagoProveedorService.ObtenerPorProveedorAsync(idProveedor, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PagoProveedorRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _pagoProveedorService.CrearAsync(request, cancellationToken);
        return Ok(response);
    }
}
