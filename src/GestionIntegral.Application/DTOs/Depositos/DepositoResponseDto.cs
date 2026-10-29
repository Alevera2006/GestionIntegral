namespace GestionIntegral.Application.DTOs.Depositos;

public class DepositoResponseDto
{
    public int IdDeposito { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
    public bool Activo { get; set; }
}
