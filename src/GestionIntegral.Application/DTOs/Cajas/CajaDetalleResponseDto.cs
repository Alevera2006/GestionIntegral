namespace GestionIntegral.Application.DTOs.Cajas;

public class CajaDetalleResponseDto
{
    public long IdCajaDetalle { get; set; }
    public long IdCaja { get; set; }
    public int IdTipoMovimientoCaja { get; set; }
    public string TipoMovimientoCajaDescripcion { get; set; } = string.Empty;
    public int? IdMedioPago { get; set; }
    public string? MedioPagoDescripcion { get; set; }
    public int? IdMoneda { get; set; }
    public string? MonedaCodigo { get; set; }
    public DateTime FechaMovimiento { get; set; }
    public string Concepto { get; set; } = string.Empty;
    public string? Referencia { get; set; }
    public decimal Importe { get; set; }
}
