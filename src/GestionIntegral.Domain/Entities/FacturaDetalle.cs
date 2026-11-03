namespace GestionIntegral.Domain.Entities;

public class FacturaDetalle
{
    public long IdFacturaDetalle { get; set; }
    public long IdFactura { get; set; }
    public int IdProducto { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal PorcentajeIva { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ImporteIva { get; set; }

    public virtual Factura Factura { get; set; } = null!;
    public virtual Producto Producto { get; set; } = null!;
}
