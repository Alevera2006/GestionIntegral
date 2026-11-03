namespace GestionIntegral.Application.DTOs.Stock;

public class StockFiltroRequestDto
{
    public int? IdDeposito { get; set; }
    public int? IdCategoria { get; set; }
    public int? IdSubCategoria { get; set; }
    public string? BusquedaProducto { get; set; }
}
