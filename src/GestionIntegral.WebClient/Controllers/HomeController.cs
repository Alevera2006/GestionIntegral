using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class HomeController : BaseController
{
    public HomeController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index()
    {
        return View();
    }
}
