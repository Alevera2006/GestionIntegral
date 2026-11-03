namespace GestionIntegral.WebClient.Models.Stock;

public class DepositoResponseDto
{
    public int IdDeposito { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
