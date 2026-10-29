namespace GestionIntegral.WebClient.Models.Auth;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public int IdTipoUsuario { get; set; }
}
