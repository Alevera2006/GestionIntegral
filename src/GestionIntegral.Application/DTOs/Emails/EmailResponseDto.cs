namespace GestionIntegral.Application.DTOs.Emails;

public class EmailResponseDto
{
    public int IdEmail { get; set; }
    public string TipoReferencia { get; set; } = string.Empty;
    public int IdReferencia { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
