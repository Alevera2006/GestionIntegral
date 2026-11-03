using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.Stock;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class ReportesController : BaseController
{
    public ReportesController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index()
    {
        var opciones = new List<OpcionStockViewModel>
        {
            new()
            {
                Titulo = "Ventas",
                Descripcion = "Reportes de facturación y ventas",
                Icono = "fa-chart-bar",
                Color = "bg-primary",
                Url = "/Reportes/Ventas"
            },
            new()
            {
                Titulo = "Compras",
                Descripcion = "Reportes de compras y proveedores",
                Icono = "fa-shopping-basket",
                Color = "bg-secondary",
                Url = "/Reportes/Compras"
            },
            new()
            {
                Titulo = "Stock",
                Descripcion = "Reportes de inventario y movimientos",
                Icono = "fa-boxes",
                Color = "bg-info",
                Url = "/Reportes/Stock"
            },
            new()
            {
                Titulo = "Finanzas",
                Descripcion = "Reportes de cobranzas y valores",
                Icono = "fa-chart-line",
                Color = "bg-success",
                Url = "/Reportes/Finanzas"
            }
        };

        return View(opciones);
    }

    public IActionResult Ventas()
    {
        var opciones = new List<OpcionStockViewModel>
        {
            new() { Titulo = "Ventas por Período", Descripcion = "Histórico de ventas por fechas", Icono = "fa-calendar-alt", Color = "bg-primary", Url = "/Reportes/VentasPorPeriodo" },
            new() { Titulo = "Ventas por Cliente", Descripcion = "Ventas agrupadas por cliente", Icono = "fa-user", Color = "bg-info", Url = "/Reportes/VentasPorCliente" },
            new() { Titulo = "Ventas por Producto", Descripcion = "Ventas agrupadas por producto", Icono = "fa-box", Color = "bg-warning", Url = "/Reportes/VentasPorProducto" },
            new() { Titulo = "Ranking de Productos", Descripcion = "Productos más vendidos", Icono = "fa-trophy", Color = "bg-success", Url = "/Reportes/RankingProductos" }
        };
        return View(opciones);
    }

    public IActionResult Compras()
    {
        var opciones = new List<OpcionStockViewModel>
        {
            new() { Titulo = "Compras por Período", Descripcion = "Histórico de compras por fechas", Icono = "fa-calendar-alt", Color = "bg-secondary", Url = "/Reportes/ComprasPorPeriodo" },
            new() { Titulo = "Compras por Proveedor", Descripcion = "Compras agrupadas por proveedor", Icono = "fa-truck", Color = "bg-primary", Url = "/Reportes/ComprasPorProveedor" }
        };
        return View(opciones);
    }

    public IActionResult Stock()
    {
        var opciones = new List<OpcionStockViewModel>
        {
            new() { Titulo = "Stock por Depósito", Descripcion = "Stock actual por depósito", Icono = "fa-warehouse", Color = "bg-info", Url = "/Reportes/StockPorDeposito" },
            new() { Titulo = "Movimientos de Stock", Descripcion = "Histórico de movimientos", Icono = "fa-dolly", Color = "bg-primary", Url = "/Reportes/MovimientosStock" },
            new() { Titulo = "Alertas de Stock", Descripcion = "Productos bajo stock mínimo", Icono = "fa-exclamation-triangle", Color = "bg-warning", Url = "/Reportes/AlertasStock" }
        };
        return View(opciones);
    }

    public IActionResult Finanzas()
    {
        var opciones = new List<OpcionStockViewModel>
        {
            new() { Titulo = "Cuentas Corrientes", Descripcion = "Saldos por cliente", Icono = "fa-book", Color = "bg-info", Url = "/Reportes/CuentasCorrientes" },
            new() { Titulo = "Cobranzas", Descripcion = "Cobranzas por período", Icono = "fa-hand-holding-usd", Color = "bg-success", Url = "/Reportes/Cobranzas" },
            new() { Titulo = "Valores en Cartera", Descripcion = "Cheques y valores disponibles", Icono = "fa-money-check-alt", Color = "bg-warning", Url = "/Reportes/ValoresCartera" },
            new() { Titulo = "Deuda por Cliente", Descripcion = "Clientes con saldo pendiente", Icono = "fa-file-invoice-dollar", Color = "bg-danger", Url = "/Reportes/DeudaPorCliente" }
        };
        return View(opciones);
    }

    // Placeholders para reportes específicos
    public IActionResult VentasPorPeriodo() => View("ReportePlaceholder", ("Ventas por Período", "Reporte de ventas por período."));
    public IActionResult VentasPorCliente() => View("ReportePlaceholder", ("Ventas por Cliente", "Reporte de ventas agrupadas por cliente."));
    public IActionResult VentasPorProducto() => View("ReportePlaceholder", ("Ventas por Producto", "Reporte de ventas agrupadas por producto."));
    public IActionResult RankingProductos() => View("ReportePlaceholder", ("Ranking de Productos", "Productos más vendidos."));
    public IActionResult ComprasPorPeriodo() => View("ReportePlaceholder", ("Compras por Período", "Reporte de compras por período."));
    public IActionResult ComprasPorProveedor() => View("ReportePlaceholder", ("Compras por Proveedor", "Reporte de compras agrupadas por proveedor."));
    public IActionResult StockPorDeposito() => View("ReportePlaceholder", ("Stock por Depósito", "Stock actual por depósito."));
    public IActionResult MovimientosStock() => View("ReportePlaceholder", ("Movimientos de Stock", "Histórico de movimientos de stock."));
    public IActionResult AlertasStock() => View("ReportePlaceholder", ("Alertas de Stock", "Productos con stock bajo el mínimo."));
    public IActionResult CuentasCorrientes() => View("ReportePlaceholder", ("Cuentas Corrientes", "Saldos de cuentas corrientes por cliente."));
    public IActionResult Cobranzas() => View("ReportePlaceholder", ("Cobranzas", "Cobranzas registradas por período."));
    public IActionResult ValoresCartera() => View("ReportePlaceholder", ("Valores en Cartera", "Cheques y otros valores en cartera."));
    public IActionResult DeudaPorCliente() => View("ReportePlaceholder", ("Deuda por Cliente", "Clientes con deuda pendiente."));
}
