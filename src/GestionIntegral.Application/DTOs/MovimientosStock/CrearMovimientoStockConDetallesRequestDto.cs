namespace GestionIntegral.Application.DTOs.MovimientosStock;

public class CrearMovimientoStockConDetallesRequestDto
{
    public int IdDeposito { get; set; }
    public int IdTipoMovimientoStock { get; set; }
    public int IdUsuarioResponsable { get; set; }
    public string? Referencia { get; set; }
    public string? Observaciones { get; set; }
    public List<CrearMovimientoStockDetalleItemRequestDto> Detalles { get; set; } = new();
}

public class CrearMovimientoStockDetalleItemRequestDto
{
    public int IdProducto { get; set; }
    public decimal Cantidad { get; set; }
}
