using GestionIntegral.Application.DTOs.Cajas;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
public class CajasController : BaseController
{
    private readonly ICajaCabeceraService _cajaService;
    private readonly ICajaDetalleService _detalleService;

    public CajasController(ICajaCabeceraService cajaService, ICajaDetalleService detalleService)
    {
        _cajaService = cajaService;
        _detalleService = detalleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _cajaService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await _cajaService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearCajaCabeceraRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _cajaService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarCajaCabeceraRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _cajaService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var response = await _cajaService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpGet("{idCaja:long}/detalles")]
    public async Task<IActionResult> GetDetalles(long idCaja, CancellationToken cancellationToken)
    {
        var response = await _detalleService.ObtenerPorCajaAsync(idCaja, cancellationToken);
        return Result(response);
    }

    [HttpPost("detalles")]
    public async Task<IActionResult> CreateDetalle([FromBody] CrearCajaDetalleRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _detalleService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("detalles/{id:long}")]
    public async Task<IActionResult> DeleteDetalle(long id, CancellationToken cancellationToken)
    {
        var response = await _detalleService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
