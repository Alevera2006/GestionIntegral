namespace GestionIntegral.WebClient.Models.Usuarios;

public class ActualizarUsuarioRequestDto
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public int IdTipoUsuario { get; set; }
    public bool Bloqueado { get; set; }
    public bool Activo { get; set; }
}
