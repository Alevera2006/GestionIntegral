using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.TiposUsuario;
using GestionIntegral.WebClient.Models.Usuarios;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class UsuariosController : BaseController
{
    public UsuariosController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<UsuarioResponseDto>>>("api/usuarios", cancellationToken);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> TiposUsuario(CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<TipoUsuarioResponseDto>>>("api/tiposusuario", cancellationToken);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearUsuarioRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PostAsync<CrearUsuarioRequestDto, BaseResponseDto<int>>("api/usuarios", dto, cancellationToken);
        return Json(response);
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar([FromBody] ActualizarUsuarioRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await ApiClient.PutAsync<ActualizarUsuarioRequestDto, BaseResponseDto<bool>>("api/usuarios", dto, cancellationToken);
        return Json(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        var ok = await ApiClient.DeleteAsync($"api/usuarios/{id}", cancellationToken);
        return Json(BaseResponseDto<bool>.Ok(ok));
    }
}
