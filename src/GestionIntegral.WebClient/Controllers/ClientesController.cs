using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.Clientes;
using GestionIntegral.WebClient.Models.ContactoDatos;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class ClientesController : BaseController
{
    public ClientesController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<ClienteResponseDto>>>("api/clientes", cancellationToken);
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
    public async Task<IActionResult> Crear([FromBody] CrearClienteRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PostAsync<CrearClienteRequestDto, BaseResponseDto<ClienteResponseDto>>("api/clientes", dto, cancellationToken);
        return Json(response);
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar([FromBody] ActualizarClienteRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PutAsync<ActualizarClienteRequestDto, BaseResponseDto<ClienteResponseDto>>("api/clientes", dto, cancellationToken);
        return Json(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        var response = await ApiClient.DeleteAsync($"api/clientes/{id}", cancellationToken);
        return Json(response
            ? BaseResponseDto<bool>.Ok(true)
            : BaseResponseDto<bool>.Error("No se pudo eliminar el cliente."));
    }

    [HttpGet]
    public async Task<IActionResult> ListarDomicilios(int idCliente, CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<DomicilioResponseDto>>>($"api/domicilios?tipoReferencia=C&idReferencia={idCliente}", cancellationToken);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> CrearDomicilio([FromBody] CrearDomicilioRequestDto dto, CancellationToken cancellationToken)
    {
        dto.TipoReferencia = "C";
        dto.IdReferencia = dto.IdReferencia > 0 ? dto.IdReferencia : (dto.IdCliente ?? 0);

        var response = await ApiClient.PostAsync<CrearDomicilioRequestDto, BaseResponseDto<DomicilioResponseDto>>("api/domicilios", dto, cancellationToken);
        return Json(response);
    }

    [HttpPut]
    public async Task<IActionResult> ActualizarDomicilio([FromBody] ActualizarDomicilioRequestDto dto, CancellationToken cancellationToken)
    {
        dto.TipoReferencia = "C";
        dto.IdReferencia = dto.IdReferencia > 0 ? dto.IdReferencia : (dto.IdCliente ?? 0);

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
    public async Task<IActionResult> ListarTelefonos(int idCliente, CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<TelefonoResponseDto>>>($"api/telefonos?tipoReferencia=C&idReferencia={idCliente}", cancellationToken);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> CrearTelefono([FromBody] CrearTelefonoRequestDto dto, CancellationToken cancellationToken)
    {
        dto.TipoReferencia = "C";
        dto.IdReferencia = dto.IdReferencia > 0 ? dto.IdReferencia : (dto.IdCliente ?? 0);

        var response = await ApiClient.PostAsync<CrearTelefonoRequestDto, BaseResponseDto<TelefonoResponseDto>>("api/telefonos", dto, cancellationToken);
        return Json(response);
    }

    [HttpPut]
    public async Task<IActionResult> ActualizarTelefono([FromBody] ActualizarTelefonoRequestDto dto, CancellationToken cancellationToken)
    {
        dto.TipoReferencia = "C";
        dto.IdReferencia = dto.IdReferencia > 0 ? dto.IdReferencia : (dto.IdCliente ?? 0);

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
    public async Task<IActionResult> ListarEmails(int idCliente, CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<EmailResponseDto>>>($"api/emails?tipoReferencia=C&idReferencia={idCliente}", cancellationToken);
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
