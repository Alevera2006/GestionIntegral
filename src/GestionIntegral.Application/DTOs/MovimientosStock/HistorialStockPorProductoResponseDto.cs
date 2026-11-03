namespace GestionIntegral.Application.DTOs.MovimientosStock;

public class HistorialStockPorProductoResponseDto
{
    public long IdMovimientoStock { get; set; }
    public long IdMovimientoStockDetalle { get; set; }
    public DateTime FechaMovimiento { get; set; }
    public int IdDeposito { get; set; }
    public string DepositoDescripcion { get; set; } = string.Empty;
    public int IdTipoMovimientoStock { get; set; }
    public string TipoMovimientoStockDescripcion { get; set; } = string.Empty;
    public short Signo { get; set; }
    public decimal Cantidad { get; set; }
    public decimal CantidadAfectada => Cantidad * Signo;
    public int IdUsuarioResponsable { get; set; }
    public string NombreUsuarioResponsable { get; set; } = string.Empty;
    public string? Referencia { get; set; }
    public string? Observaciones { get; set; }
}
