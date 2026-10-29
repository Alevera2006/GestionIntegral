using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.TiposUsuario;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class TiposUsuarioController : BaseController
{
    public TiposUsuarioController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<TipoUsuarioResponseDto>>>("api/tiposusuario", cancellationToken);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearTipoUsuarioRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PostAsync<CrearTipoUsuarioRequestDto, BaseResponseDto<TipoUsuarioResponseDto>>("api/tiposusuario", dto, cancellationToken);
        return Json(response);
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar([FromBody] TipoUsuarioResponseDto dto, CancellationToken cancellationToken)
    {
        // La API no tiene PUT para tipos de usuario; simulamos con id + descripción no implementado en backend.
        // Se deja preparado para cuando se agregue.
        return Json(BaseResponseDto<bool>.Error("La API no soporta actualización de tipos de usuario."));
    }

    [HttpDelete]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        var ok = await ApiClient.DeleteAsync($"api/tiposusuario/{id}", cancellationToken);
        return Json(BaseResponseDto<bool>.Ok(ok));
    }
}
