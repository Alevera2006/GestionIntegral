using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.Stock;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class ComprasController : BaseController
{
    public ComprasController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index()
    {
        var opciones = new List<OpcionStockViewModel>
        {
            new()
            {
                Titulo = "Proveedores",
                Descripcion = "Administrar proveedores",
                Icono = "fa-truck",
                Color = "bg-secondary",
                Url = "/Proveedores"
            }
        };

        return View(opciones);
    }
}
