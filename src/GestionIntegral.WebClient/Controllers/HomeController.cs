using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.Home;
using GestionIntegral.WebClient.Models.Parametros;
using GestionIntegral.WebClient.Models.TiposUsuario;
using GestionIntegral.WebClient.Models.Usuarios;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class HomeController : BaseController
{
    public HomeController(IApiClient apiClient) : base(apiClient)
    {
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var vm = new DashboardViewModel
        {
            UsuarioNombre = User.Identity?.Name ?? "Usuario",
            AccesosRapidos = new List<AccesoRapidoViewModel>
            {
                new() { Titulo = "Usuarios", Descripcion = "Gestionar usuarios", Icono = "fa-users", Color = "bg-info", Url = "/Usuarios" },
                new() { Titulo = "Tipos de Usuario", Descripcion = "Gestionar tipos", Icono = "fa-user-tag", Color = "bg-success", Url = "/TiposUsuario" },
                new() { Titulo = "Parámetros", Descripcion = "Configuración general", Icono = "fa-sliders-h", Color = "bg-warning", Url = "/Parametros" }
            }
        };

        try
        {
            var usuariosTask = ApiClient.GetAsync<BaseResponseDto<IEnumerable<UsuarioResponseDto>>>("api/usuarios", cancellationToken);
            var tiposTask = ApiClient.GetAsync<BaseResponseDto<IEnumerable<TipoUsuarioResponseDto>>>("api/tiposusuario", cancellationToken);

            await Task.WhenAll(usuariosTask, tiposTask);

            var usuarios = usuariosTask.Result?.Datos ?? Enumerable.Empty<UsuarioResponseDto>();
            var tipos = tiposTask.Result?.Datos ?? Enumerable.Empty<TipoUsuarioResponseDto>();

            vm.TotalUsuarios = usuarios.Count();
            vm.UsuariosActivos = usuarios.Count(u => u.Activo);
            vm.TotalTiposUsuario = tipos.Count();
            vm.TotalParametros = 4; // Provincias, TiposComprobante, MotivosAnulacion, EstadosPedido
        }
        catch
        {
            // Si falla alguna API, dejamos los contadores en 0
        }

        return View(vm);
    }
}
