namespace GestionIntegral.Application.DTOs.ClienteContactos;

public class ClienteContactoResponseDto
{
    public int IdCliente { get; set; }
    public string ClienteRazonSocial { get; set; } = string.Empty;
    public int IdContacto { get; set; }
    public string ContactoNombre { get; set; } = string.Empty;
    public bool EsPrincipal { get; set; }
}
