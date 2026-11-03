namespace GestionIntegral.WebClient.Models.PagosProveedores;

public class PagoProveedorResponseDto
{
    public long IdPagoProveedor { get; set; }
    public int IdProveedor { get; set; }
    public string ProveedorNombre { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public decimal Monto { get; set; }
    public string MedioPago { get; set; } = string.Empty;
    public string? NumeroCheque { get; set; }
    public string? Banco { get; set; }
    public DateTime? FechaVencimientoCheque { get; set; }
    public string Concepto { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
}
