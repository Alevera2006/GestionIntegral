using GestionIntegral.Application.DTOs.Productos;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class ProductosController : BaseController
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _productoService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _productoService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearProductoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _productoService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarProductoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _productoService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _productoService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
