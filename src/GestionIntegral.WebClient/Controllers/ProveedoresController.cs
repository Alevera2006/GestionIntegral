using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.ContactoDatos;
using GestionIntegral.WebClient.Models.Proveedores;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class ProveedoresController : BaseController
{
    public ProveedoresController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<ProveedorResponseDto>>>("api/proveedores", cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> CondicionesPago(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<CondicionPagoResponseDto>>>("api/condicionespago", cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> Provincias(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<ProvinciaResponseDto>>>("api/provincias", cancellationToken);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearProveedorRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PostAsync<CrearProveedorRequestDto, BaseResponseDto<ProveedorResponseDto>>("api/proveedores", dto, cancellationToken);
        return Json(response);
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar([FromBody] ActualizarProveedorRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PutAsync<ActualizarProveedorRequestDto, BaseResponseDto<ProveedorResponseDto>>("api/proveedores", dto, cancellationToken);
        return Json(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        var response = await ApiClient.DeleteAsync($"api/proveedores/{id}", cancellationToken);
        return Json(response
            ? BaseResponseDto<bool>.Ok(true)
            : BaseResponseDto<bool>.Error("No se pudo eliminar el proveedor."));
    }

    [HttpGet]
    public async Task<IActionResult> ListarDomicilios(int idProveedor, CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<DomicilioResponseDto>>>($"api/domicilios?tipoReferencia=P&idReferencia={idProveedor}", cancellationToken);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> CrearDomicilio([FromBody] CrearDomicilioRequestDto dto, CancellationToken cancellationToken)
    {
        dto.TipoReferencia = "P";
        dto.IdReferencia = dto.IdReferencia > 0 ? dto.IdReferencia : (dto.IdProveedor ?? 0);

        var response = await ApiClient.PostAsync<CrearDomicilioRequestDto, BaseResponseDto<DomicilioResponseDto>>("api/domicilios", dto, cancellationToken);
        return Json(response);
    }

    [HttpPut]
    public async Task<IActionResult> ActualizarDomicilio([FromBody] ActualizarDomicilioRequestDto dto, CancellationToken cancellationToken)
    {
        dto.TipoReferencia = "P";
        dto.IdReferencia = dto.IdReferencia > 0 ? dto.IdReferencia : (dto.IdProveedor ?? 0);

        var response = await ApiClient.PutAsync<ActualizarDomicilioRequestDto, BaseResponseDto<DomicilioResponseDto>>("api/domicilios", dto, cancellationToken);
        return Json(response);
    }

    [HttpDelete]
    public async Task<IActionResult> EliminarDomicilio(int id, CancellationToken cancellationToken)
    {
        var ok = await ApiClient.DeleteAsync($"api/domicilios/{id}", cancellationToken);
        return Json(ok ? BaseResponseDto<bool>.Ok(true) : BaseResponseDto<bool>.Error("No se pudo eliminar el domicilio."));
    }

    [HttpGet]
    public async Task<IActionResult> ListarTelefonos(int idProveedor, CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<TelefonoResponseDto>>>($"api/telefonos?tipoReferencia=P&idReferencia={idProveedor}", cancellationToken);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> CrearTelefono([FromBody] CrearTelefonoRequestDto dto, CancellationToken cancellationToken)
    {
        dto.TipoReferencia = "P";
        dto.IdReferencia = dto.IdReferencia > 0 ? dto.IdReferencia : (dto.IdProveedor ?? 0);

        var response = await ApiClient.PostAsync<CrearTelefonoRequestDto, BaseResponseDto<TelefonoResponseDto>>("api/telefonos", dto, cancellationToken);
        return Json(response);
    }

    [HttpPut]
    public async Task<IActionResult> ActualizarTelefono([FromBody] ActualizarTelefonoRequestDto dto, CancellationToken cancellationToken)
    {
        dto.TipoReferencia = "P";
        dto.IdReferencia = dto.IdReferencia > 0 ? dto.IdReferencia : (dto.IdProveedor ?? 0);

        var response = await ApiClient.PutAsync<ActualizarTelefonoRequestDto, BaseResponseDto<TelefonoResponseDto>>("api/telefonos", dto, cancellationToken);
        return Json(response);
    }

    [HttpDelete]
    public async Task<IActionResult> EliminarTelefono(int id, CancellationToken cancellationToken)
    {
        var ok = await ApiClient.DeleteAsync($"api/telefonos/{id}", cancellationToken);
        return Json(ok ? BaseResponseDto<bool>.Ok(true) : BaseResponseDto<bool>.Error("No se pudo eliminar el teléfono."));
    }

    [HttpGet]
    public async Task<IActionResult> ListarEmails(int idProveedor, CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<EmailResponseDto>>>($"api/emails?tipoReferencia=P&idReferencia={idProveedor}", cancellationToken);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> CrearEmail([FromBody] CrearEmailRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PostAsync<CrearEmailRequestDto, BaseResponseDto<EmailResponseDto>>("api/emails", dto, cancellationToken);
        return Json(response);
    }

    [HttpPut]
    public async Task<IActionResult> ActualizarEmail([FromBody] ActualizarEmailRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PutAsync<ActualizarEmailRequestDto, BaseResponseDto<EmailResponseDto>>("api/emails", dto, cancellationToken);
        return Json(response);
    }

    [HttpDelete]
    public async Task<IActionResult> EliminarEmail(int id, CancellationToken cancellationToken)
    {
        var ok = await ApiClient.DeleteAsync($"api/emails/{id}", cancellationToken);
        return Json(ok ? BaseResponseDto<bool>.Ok(true) : BaseResponseDto<bool>.Error("No se pudo eliminar el email."));
    }
}
