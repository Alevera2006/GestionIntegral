namespace GestionIntegral.Application.DTOs.Contactos;

public class ContactoResponseDto
{
    public int IdContacto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Apellido { get; set; }
    public string? Cargo { get; set; }
    public string? Telefono { get; set; }
    public string? Celular { get; set; }
    public string? Email { get; set; }
    public bool Activo { get; set; }
}
