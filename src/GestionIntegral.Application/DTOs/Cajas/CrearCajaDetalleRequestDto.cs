namespace GestionIntegral.Application.DTOs.Cajas;

public class CrearCajaDetalleRequestDto
{
    public long IdCaja { get; set; }
    public int IdTipoMovimientoCaja { get; set; }
    public int? IdMedioPago { get; set; }
    public int? IdMoneda { get; set; }
    public string Concepto { get; set; } = string.Empty;
    public string? Referencia { get; set; }
    public decimal Importe { get; set; }
}
