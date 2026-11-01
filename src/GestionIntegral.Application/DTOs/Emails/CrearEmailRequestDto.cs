namespace GestionIntegral.Application.DTOs.Emails;

public class CrearEmailRequestDto
{
    public string TipoReferencia { get; set; } = string.Empty;
    public int IdReferencia { get; set; }
    public string Email { get; set; } = string.Empty;
}
