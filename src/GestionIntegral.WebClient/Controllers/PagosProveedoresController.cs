using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.ContactoDatos;
using GestionIntegral.WebClient.Models.PagosProveedores;
using GestionIntegral.WebClient.Models.Proveedores;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class PagosProveedoresController : BaseController
{
    public PagosProveedoresController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Proveedores(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<ProveedorResponseDto>>>("api/proveedores", cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> MediosPago(CancellationToken cancellationToken)
    {
        try
        {
            var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<MedioPagoResponseDto>>>("api/mediospago", cancellationToken);
            return Json(response);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return StatusCode(499);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<PagoProveedorResponseDto>>>("api/pagosproveedores", cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> PorProveedor(int idProveedor, CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<PagoProveedorResponseDto>>>($"api/pagosproveedores/proveedor/{idProveedor}", cancellationToken);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] PagoProveedorRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PostAsync<PagoProveedorRequestDto, BaseResponseDto<PagoProveedorResponseDto>>("api/pagosproveedores", dto, cancellationToken);
        return Json(response);
    }
}
