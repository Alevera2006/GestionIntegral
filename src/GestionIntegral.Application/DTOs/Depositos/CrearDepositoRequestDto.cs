namespace GestionIntegral.Application.DTOs.Depositos;

public class CrearDepositoRequestDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
}
