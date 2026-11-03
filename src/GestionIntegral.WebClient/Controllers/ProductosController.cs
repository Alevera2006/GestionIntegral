using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.Productos;
using GestionIntegral.WebClient.Models.Stock;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class ProductosController : BaseController
{
    public ProductosController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<ProductoResponseDto>>>("api/productos", cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> Categorias(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<CategoriaResponseDto>>>("api/categorias", cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> SubCategorias(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<SubCategoriaResponseDto>>>("api/subcategorias", cancellationToken);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearProductoRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PostAsync<CrearProductoRequestDto, BaseResponseDto<ProductoResponseDto>>("api/productos", dto, cancellationToken);
        return Json(response);
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar([FromBody] ActualizarProductoRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PutAsync<ActualizarProductoRequestDto, BaseResponseDto<ProductoResponseDto>>("api/productos", dto, cancellationToken);
        return Json(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        var ok = await ApiClient.DeleteAsync($"api/productos/{id}", cancellationToken);
        return Json(ok
            ? BaseResponseDto<bool>.Ok(true)
            : BaseResponseDto<bool>.Error("No se pudo eliminar el producto."));
    }
}
