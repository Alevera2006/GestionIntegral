using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.Stock;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class FinanzasController : BaseController
{
    public FinanzasController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index()
    {
        var opciones = new List<OpcionStockViewModel>
        {
            new()
            {
                Titulo = "Cuentas Corrientes",
                Descripcion = "Consultar movimientos de clientes",
                Icono = "fa-book",
                Color = "bg-info",
                Url = "/CuentasCorrientes"
            },
            new()
            {
                Titulo = "Cobranzas",
                Descripcion = "Registrar pagos de clientes",
                Icono = "fa-hand-holding-usd",
                Color = "bg-success",
                Url = "/CuentasCorrientes?accion=cobranza"
            },
            new()
            {
                Titulo = "Valores en Cartera",
                Descripcion = "Cheques y otros valores",
                Icono = "fa-money-check-alt",
                Color = "bg-warning",
                Url = "/ValoresCartera"
            }
        };

        return View(opciones);
    }
}
