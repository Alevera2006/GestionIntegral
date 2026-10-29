using System.Text.Json;
using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.Parametros;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class ParametrosController : BaseController
{
    public ParametrosController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index()
    {
        var tarjetas = new List<TarjetaParametroViewModel>
        {
            new() { Key = "provincias", Titulo = "Provincias", Icono = "fa-map-marker-alt", Color = "bg-primary", ApiUrl = "api/provincias", RequiereCodigoAFIP = true },
            new() { Key = "tiposComprobante", Titulo = "Tipos de Comprobante", Icono = "fa-file-invoice", Color = "bg-success", ApiUrl = "api/tiposcomprobante", RequiereCodigoAFIP = true },
            new() { Key = "motivosAnulacion", Titulo = "Motivos de Anulación", Icono = "fa-ban", Color = "bg-warning", ApiUrl = "api/motivosanulacion", RequiereCodigoAFIP = false },
            new() { Key = "estadosPedido", Titulo = "Estados de Pedido", Icono = "fa-truck", Color = "bg-info", ApiUrl = "api/estadospedido", RequiereCodigoAFIP = false }
        };

        return View(tarjetas);
    }

    [HttpGet]
    public async Task<IActionResult> Listar(string apiUrl, CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<ParametroItemDto>>>(apiUrl, cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> Obtener(string apiUrl, int id, CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<ParametroItemDto>>($"{apiUrl}/{id}", cancellationToken);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(string apiUrl, [FromBody] ParametroFormDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PostAsync<ParametroFormDto, BaseResponseDto<JsonElement>>(apiUrl, dto, cancellationToken);
        return Json(response);
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar(string apiUrl, [FromBody] ParametroFormDto dto, CancellationToken cancellationToken)
    {
        var idProperty = apiUrl switch
        {
            "api/provincias" => "IdProvincia",
            "api/tiposcomprobante" => "IdTipoComprobante",
            "api/motivosanulacion" => "IdMotivoAnulacion",
            "api/estadospedido" => "IdEstadoPedido",
            _ => null
        };

        if (idProperty is null || dto.Id is null or <= 0)
            return BadRequest(BaseResponseDto<JsonElement>.Error("El parámetro o su identificador no es válido."));

        var request = new Dictionary<string, object?>
        {
            [idProperty] = dto.Id.Value,
            ["Codigo"] = dto.Codigo,
            ["Descripcion"] = dto.Descripcion,
            ["CodigoAFIP"] = dto.CodigoAFIP,
            ["Activo"] = dto.Activo
        };

        var response = await ApiClient.PutAsync<Dictionary<string, object?>, BaseResponseDto<JsonElement>>(apiUrl, request, cancellationToken);
        return Json(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Eliminar(string apiUrl, int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest(BaseResponseDto<bool>.Error("El identificador debe ser mayor que cero."));

        var ok = await ApiClient.DeleteAsync($"{apiUrl}/{id}", cancellationToken);
        return Json(ok
            ? BaseResponseDto<bool>.Ok(true)
            : BaseResponseDto<bool>.Error("No se pudo eliminar el registro; puede que ya esté dado de baja."));
    }
}
