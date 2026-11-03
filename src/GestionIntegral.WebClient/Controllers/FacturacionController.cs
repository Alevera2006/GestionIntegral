using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.Clientes;
using GestionIntegral.WebClient.Models.ContactoDatos;
using GestionIntegral.WebClient.Models.Facturacion;
using GestionIntegral.WebClient.Models.Parametros;
using GestionIntegral.WebClient.Models.Productos;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class FacturacionController : BaseController
{
    public FacturacionController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Clientes(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<ClienteResponseDto>>>("api/clientes", cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> Productos(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<ProductoResponseDto>>>("api/productos", cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> CondicionesPago(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<CondicionPagoResponseDto>>>("api/condicionespago", cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> TiposComprobante(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<TipoComprobanteResponseDto>>>("api/tiposcomprobante", cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> AlicuotasIva(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<AlicuotaIvaResponseDto>>>("api/alicuotasimpuesto", cancellationToken);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] FacturaRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PostAsync<FacturaRequestDto, BaseResponseDto<FacturaResponseDto>>("api/facturacion", dto, cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<FacturaResponseDto>>>("api/facturacion", cancellationToken);
        return Json(response);
    }
}
