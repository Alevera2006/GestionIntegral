namespace GestionIntegral.Domain.Entities;

public class PagoProveedor : EntityBase
{
    public long IdPagoProveedor { get; set; }
    public int IdProveedor { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Monto { get; set; }
    public int IdMedioPago { get; set; }
    public string? NumeroCheque { get; set; }
    public string? Banco { get; set; }
    public DateTime? FechaVencimientoCheque { get; set; }
    public string Concepto { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public virtual Proveedor Proveedor { get; set; } = null!;
    public virtual MedioPago MedioPago { get; set; } = null!;
}
