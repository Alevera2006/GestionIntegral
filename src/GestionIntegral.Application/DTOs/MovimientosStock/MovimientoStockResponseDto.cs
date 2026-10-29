namespace GestionIntegral.Application.DTOs.MovimientosStock;

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
}
