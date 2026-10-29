namespace GestionIntegral.Application.DTOs.MovimientosStock;

public class ActualizarMovimientoStockRequestDto
{
    public long IdMovimientoStock { get; set; }
    public int IdDeposito { get; set; }
    public int IdTipoMovimientoStock { get; set; }
    public int IdUsuarioResponsable { get; set; }
    public string? Referencia { get; set; }
    public string? Observaciones { get; set; }
}
