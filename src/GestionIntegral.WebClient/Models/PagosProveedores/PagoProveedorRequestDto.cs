namespace GestionIntegral.WebClient.Models.PagosProveedores;

public class PagoProveedorRequestDto
{
    public int IdProveedor { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Today;
    public decimal Monto { get; set; }
    public int IdMedioPago { get; set; }
    public string? NumeroCheque { get; set; }
    public string? Banco { get; set; }
    public DateTime? FechaVencimientoCheque { get; set; }
    public string Concepto { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
}
