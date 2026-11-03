using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.Stock;
using GestionIntegral.WebClient.Models.Usuarios;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class StockController : BaseController
{
    public StockController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index()
    {
        var opciones = new List<OpcionStockViewModel>
        {
            new()
            {
                Titulo = "Stock Actual",
                Descripcion = "Consultar stock por depósito, categoría y subcategoría",
                Icono = "fa-boxes",
                Color = "bg-info",
                Url = "/Stock/Actual"
            },
            new()
            {
                Titulo = "Movimientos",
                Descripcion = "Listado histórico y registro de movimientos de stock",
                Icono = "fa-dolly",
                Color = "bg-primary",
                Url = "/Stock/Movimientos"
            }
        };

        return View(opciones);
    }

    public async Task<IActionResult> Actual(CancellationToken cancellationToken)
    {
        var depositos = await CargarDepositosAsync(cancellationToken);
        var categorias = await CargarCategoriasAsync(cancellationToken);
        var subcategorias = await CargarSubCategoriasAsync(cancellationToken);

        ViewBag.Depositos = depositos;
        ViewBag.Categorias = categorias;
        ViewBag.SubCategorias = subcategorias;

        return View();
    }

    public IActionResult Movimientos()
    {
        return View();
    }

    public async Task<IActionResult> NuevoMovimiento(CancellationToken cancellationToken)
    {
        var depositos = await CargarDepositosAsync(cancellationToken);
        var tiposMovimiento = await CargarTiposMovimientoAsync(cancellationToken);
        var usuarios = await CargarUsuariosAsync(cancellationToken);

        ViewBag.Depositos = depositos;
        ViewBag.TiposMovimiento = tiposMovimiento;
        ViewBag.Usuarios = usuarios;

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Filtrar(int? idDeposito, int? idCategoria, int? idSubCategoria, string? busquedaProducto, CancellationToken cancellationToken)
    {
        var query = new List<string>();
        if (idDeposito.HasValue) query.Add($"IdDeposito={idDeposito.Value}");
        if (idCategoria.HasValue) query.Add($"IdCategoria={idCategoria.Value}");
        if (idSubCategoria.HasValue) query.Add($"IdSubCategoria={idSubCategoria.Value}");
        if (!string.IsNullOrWhiteSpace(busquedaProducto)) query.Add($"BusquedaProducto={Uri.EscapeDataString(busquedaProducto)}");

        var url = "api/stock/filtrar" + (query.Count > 0 ? "?" + string.Join("&", query) : string.Empty);
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<StockResponseDto>>>(url, cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> ListarMovimientos(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<MovimientoStockResponseDto>>>("api/movimientosstock", cancellationToken);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> CrearMovimiento([FromBody] CrearMovimientoStockRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PostAsync<CrearMovimientoStockRequestDto, BaseResponseDto<MovimientoStockResponseDto>>("api/movimientosstock/con-detalles", dto, cancellationToken);
        return Json(response);
    }

    [HttpDelete]
    public async Task<IActionResult> EliminarMovimiento(long id, CancellationToken cancellationToken)
    {
        var ok = await ApiClient.DeleteAsync($"api/movimientosstock/{id}", cancellationToken);
        return Json(ok
            ? BaseResponseDto<bool>.Ok(true)
            : BaseResponseDto<bool>.Error("No se pudo eliminar el movimiento."));
    }

    [HttpGet]
    public async Task<IActionResult> BuscarProductos(string? busqueda, CancellationToken cancellationToken)
    {
        var url = "api/productos";
        if (!string.IsNullOrWhiteSpace(busqueda))
            url += "?busqueda=" + Uri.EscapeDataString(busqueda);

        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<ProductoLookupDto>>>(url, cancellationToken);
        return Json(response);
    }

    private async Task<List<DepositoResponseDto>> CargarDepositosAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<DepositoResponseDto>>>("api/depositos", cancellationToken);
            return response?.Datos?.Where(d => d.Activo).ToList() ?? new List<DepositoResponseDto>();
        }
        catch
        {
            return new List<DepositoResponseDto>();
        }
    }

    private async Task<List<CategoriaResponseDto>> CargarCategoriasAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<CategoriaResponseDto>>>("api/categorias", cancellationToken);
            return response?.Datos?.Where(c => c.Activo).ToList() ?? new List<CategoriaResponseDto>();
        }
        catch
        {
            return new List<CategoriaResponseDto>();
        }
    }

    private async Task<List<SubCategoriaResponseDto>> CargarSubCategoriasAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<SubCategoriaResponseDto>>>("api/subcategorias", cancellationToken);
            return response?.Datos?.Where(s => s.Activo).ToList() ?? new List<SubCategoriaResponseDto>();
        }
        catch
        {
            return new List<SubCategoriaResponseDto>();
        }
    }

    private async Task<List<TipoMovimientoStockResponseDto>> CargarTiposMovimientoAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<TipoMovimientoStockResponseDto>>>("api/tiposmovimientostock", cancellationToken);
            return response?.Datos?.Where(t => t.Activo).ToList() ?? new List<TipoMovimientoStockResponseDto>();
        }
        catch
        {
            return new List<TipoMovimientoStockResponseDto>();
        }
    }

    private async Task<List<UsuarioResponseDto>> CargarUsuariosAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<UsuarioResponseDto>>>("api/usuarios", cancellationToken);
            return response?.Datos?.Where(u => u.Activo).ToList() ?? new List<UsuarioResponseDto>();
        }
        catch
        {
            return new List<UsuarioResponseDto>();
        }
    }
}
