namespace GestionIntegral.WebClient.Models.Usuarios;

public class CrearUsuarioRequestDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public int IdTipoUsuario { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public string Clave { get; set; } = string.Empty;
}
