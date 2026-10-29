namespace GestionIntegral.WebClient.Models.Usuarios;

public class UsuarioResponseDto
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string NombreCompleto => $"{Nombre} {Apellido}";
    public int IdTipoUsuario { get; set; }
    public string TipoUsuarioDescripcion { get; set; } = string.Empty;
    public string UsuarioNombre { get; set; } = string.Empty;
    public bool Bloqueado { get; set; }
    public bool Activo { get; set; }
}
