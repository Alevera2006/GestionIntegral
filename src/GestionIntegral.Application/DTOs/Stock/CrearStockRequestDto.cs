namespace GestionIntegral.Application.DTOs.Stock;

public class CrearStockRequestDto
{
    public int IdDeposito { get; set; }
    public int IdProducto { get; set; }
    public decimal Cantidad { get; set; }
}
