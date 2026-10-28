using GestionIntegral.Application.DTOs.ListaPrecioDetalles;
using GestionIntegral.Application.DTOs.ListasPrecios;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class ListasPreciosController : BaseController
{
    private readonly IListaPrecioService _listaPrecioService;

    public ListasPreciosController(IListaPrecioService listaPrecioService)
    {
        _listaPrecioService = listaPrecioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _listaPrecioService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _listaPrecioService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearListaPrecioRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _listaPrecioService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarListaPrecioRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _listaPrecioService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _listaPrecioService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpGet("{idListaPrecio:int}/detalles")]
    public async Task<IActionResult> GetDetalles(int idListaPrecio, CancellationToken cancellationToken)
    {
        var response = await _listaPrecioService.ObtenerDetallesAsync(idListaPrecio, cancellationToken);
        return Result(response);
    }

    [HttpPost("detalles")]
    public async Task<IActionResult> AddDetalle([FromBody] CrearListaPrecioDetalleRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _listaPrecioService.AgregarDetalleAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut("detalles")]
    public async Task<IActionResult> UpdateDetalle([FromBody] ActualizarListaPrecioDetalleRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _listaPrecioService.ActualizarDetalleAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("detalles/{idListaPrecioDetalle:int}")]
    public async Task<IActionResult> DeleteDetalle(int idListaPrecioDetalle, CancellationToken cancellationToken)
    {
        var response = await _listaPrecioService.EliminarDetalleAsync(idListaPrecioDetalle, cancellationToken);
        return Result(response);
    }
}
