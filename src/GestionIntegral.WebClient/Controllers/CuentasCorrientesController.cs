using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.ContactoDatos;
using GestionIntegral.WebClient.Models.CuentasCorrientes;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class CuentasCorrientesController : BaseController
{
    public CuentasCorrientesController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ListarClientes(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<CuentaCorrienteClienteDto>>>("api/cuentascorrientes/clientes", cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> Movimientos(int idCliente, CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<MovimientoCuentaCorrienteDto>>>($"api/cuentascorrientes/{idCliente}/movimientos", cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> MediosPago(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<MedioPagoResponseDto>>>("api/mediospago", cancellationToken);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarPago([FromBody] PagoClienteRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PostAsync<PagoClienteRequestDto, BaseResponseDto<bool>>("api/pagosclientes", dto, cancellationToken);
        return Json(response);
    }
}
