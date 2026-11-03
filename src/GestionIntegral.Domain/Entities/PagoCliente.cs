namespace GestionIntegral.Domain.Entities;

public class PagoCliente : EntityBase
{
    public long IdPagoCliente { get; set; }
    public int IdCliente { get; set; }
    public long? IdFactura { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Monto { get; set; }
    public int IdMedioPago { get; set; }
    public string? NumeroCheque { get; set; }
    public string? Banco { get; set; }
    public DateTime? FechaVencimientoCheque { get; set; }
    public string Observaciones { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public virtual Cliente Cliente { get; set; } = null!;
    public virtual Factura? Factura { get; set; }
    public virtual MedioPago MedioPago { get; set; } = null!;
    public virtual ValorCartera? ValorCartera { get; set; }
}
