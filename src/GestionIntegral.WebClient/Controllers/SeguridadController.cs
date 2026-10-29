using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class SeguridadController : BaseController
{
    public SeguridadController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index()
    {
        return View();
    }
}
