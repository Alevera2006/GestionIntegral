namespace GestionIntegral.Domain.Entities;

public class Factura : EntityBase
{
    public long IdFactura { get; set; }
    public int? IdCliente { get; set; }
    public string NombreCliente { get; set; } = "Consumidor Final";
    public int IdTipoComprobante { get; set; }
    public string NumeroComprobante { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int IdCondicionPago { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ImporteIva { get; set; }
    public decimal Total { get; set; }
    public string Observaciones { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public DateTime? FechaBaja { get; set; }

    public virtual Cliente? Cliente { get; set; }
    public virtual TipoComprobante TipoComprobante { get; set; } = null!;
    public virtual CondicionPago CondicionPago { get; set; } = null!;
    public virtual ICollection<FacturaDetalle> Detalles { get; set; } = new List<FacturaDetalle>();
    public virtual ICollection<PagoCliente> PagosAplicados { get; set; } = new List<PagoCliente>();
}
