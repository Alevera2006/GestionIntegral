namespace GestionIntegral.Application.DTOs.MovimientosStock;

public class MovimientoStockDetalleResponseDto
{
    public long IdMovimientoStockDetalle { get; set; }
    public long IdMovimientoStock { get; set; }
    public int IdProducto { get; set; }
    public string ProductoCodigo { get; set; } = string.Empty;
    public string ProductoDescripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
}
