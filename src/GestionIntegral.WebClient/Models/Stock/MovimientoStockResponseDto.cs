namespace GestionIntegral.WebClient.Models.Stock;

public class MovimientoStockResponseDto
{
    public long IdMovimientoStock { get; set; }
    public int IdDeposito { get; set; }
    public string DepositoDescripcion { get; set; } = string.Empty;
    public int IdTipoMovimientoStock { get; set; }
    public string TipoMovimientoStockDescripcion { get; set; } = string.Empty;
    public DateTime FechaMovimiento { get; set; }
    public int IdUsuarioResponsable { get; set; }
    public string NombreUsuarioResponsable { get; set; } = string.Empty;
    public string? Referencia { get; set; }
    public string? Observaciones { get; set; }
    public List<MovimientoStockDetalleResponseDto> Detalles { get; set; } = new();
}

public class MovimientoStockDetalleResponseDto
{
    public long IdMovimientoStockDetalle { get; set; }
    public long IdMovimientoStock { get; set; }
    public int IdProducto { get; set; }
    public string ProductoCodigo { get; set; } = string.Empty;
    public string ProductoDescripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
}
