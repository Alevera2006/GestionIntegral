namespace GestionIntegral.WebClient.Models.Auth;

public class LoginRequestDto
{
    public string Usuario { get; set; } = string.Empty;
    public string Clave { get; set; } = string.Empty;
    public bool Recordarme { get; set; }
}
