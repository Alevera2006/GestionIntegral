namespace GestionIntegral.Application.DTOs.Stock;

public class StockResponseDto
{
    public long IdStock { get; set; }
    public int IdDeposito { get; set; }
    public string DepositoDescripcion { get; set; } = string.Empty;
    public int IdProducto { get; set; }
    public string ProductoCodigo { get; set; } = string.Empty;
    public string ProductoDescripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public DateTime? FechaUltimoMovimiento { get; set; }
}
