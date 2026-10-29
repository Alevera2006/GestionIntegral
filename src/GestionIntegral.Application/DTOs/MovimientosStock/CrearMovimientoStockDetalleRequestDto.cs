namespace GestionIntegral.Application.DTOs.MovimientosStock;

public class CrearMovimientoStockDetalleRequestDto
{
    public long IdMovimientoStock { get; set; }
    public int IdProducto { get; set; }
    public decimal Cantidad { get; set; }
}
