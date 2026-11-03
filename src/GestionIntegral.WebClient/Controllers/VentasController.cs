using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.Stock;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class VentasController : BaseController
{
    public VentasController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index()
    {
        var opciones = new List<OpcionStockViewModel>
        {
            new()
            {
                Titulo = "Facturación",
                Descripcion = "Registrar ventas y comprobantes",
                Icono = "fa-file-invoice-dollar",
                Color = "bg-primary",
                Url = "/Facturacion"
            },
            new()
            {
                Titulo = "Productos",
                Descripcion = "Administrar productos",
                Icono = "fa-box-open",
                Color = "bg-info",
                Url = "/Productos"
            }
        };

        return View(opciones);
    }
}
