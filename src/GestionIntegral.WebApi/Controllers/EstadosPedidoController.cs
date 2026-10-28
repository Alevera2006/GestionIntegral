using GestionIntegral.Application.DTOs.EstadosPedido;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class EstadosPedidoController : BaseController
{
    private readonly IEstadoPedidoService _estadoPedidoService;

    public EstadosPedidoController(IEstadoPedidoService estadoPedidoService)
    {
        _estadoPedidoService = estadoPedidoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _estadoPedidoService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _estadoPedidoService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearEstadoPedidoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _estadoPedidoService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarEstadoPedidoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _estadoPedidoService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _estadoPedidoService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
