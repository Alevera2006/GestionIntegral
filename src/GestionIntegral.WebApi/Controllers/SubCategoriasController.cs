using GestionIntegral.Application.DTOs.SubCategorias;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
public class SubCategoriasController : BaseController
{
    private readonly ISubCategoriaService _subCategoriaService;

    public SubCategoriasController(ISubCategoriaService subCategoriaService)
    {
        _subCategoriaService = subCategoriaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _subCategoriaService.ObtenerTodosAsync(cancellationToken);
        return Result(response);
    }

    [HttpGet("categoria/{idCategoria:int}")]
    public async Task<IActionResult> GetByCategoria(int idCategoria, CancellationToken cancellationToken)
    {
        var response = await _subCategoriaService.ObtenerPorCategoriaAsync(idCategoria, cancellationToken);
        return Result(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _subCategoriaService.ObtenerPorIdAsync(id, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearSubCategoriaRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _subCategoriaService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ActualizarSubCategoriaRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _subCategoriaService.ActualizarAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _subCategoriaService.EliminarAsync(id, cancellationToken);
        return Result(response);
    }
}
