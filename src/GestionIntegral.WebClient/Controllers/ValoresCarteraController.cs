using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.ValoresCartera;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class ValoresCarteraController : BaseController
{
    public ValoresCarteraController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<ValorCarteraResponseDto>>>("api/valorescartera", cancellationToken);
        return Json(response);
    }
}
