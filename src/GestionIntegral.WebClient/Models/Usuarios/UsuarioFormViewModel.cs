using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionIntegral.WebClient.Models.Usuarios;

public class UsuarioFormViewModel
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public int IdTipoUsuario { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public string? Clave { get; set; }
    public bool Bloqueado { get; set; }
    public bool Activo { get; set; } = true;

    public List<SelectListItem> TiposUsuario { get; set; } = new();
}
