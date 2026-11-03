namespace GestionIntegral.Application.DTOs.Facturacion;

public class FacturaResponseDto
{
    public long IdFactura { get; set; }
    public int? IdCliente { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;
    public string TipoComprobante { get; set; } = string.Empty;
    public string NumeroComprobante { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string CondicionPago { get; set; } = string.Empty;
    public bool EsCuentaCorriente { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ImporteIva { get; set; }
    public decimal Total { get; set; }
    public string Observaciones { get; set; } = string.Empty;
    public List<FacturaDetalleResponseDto> Detalles { get; set; } = new();
    public bool Activo { get; set; }
}

public class FacturaDetalleResponseDto
{
    public long IdFacturaDetalle { get; set; }
    public int IdProducto { get; set; }
    public string ProductoDescripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal PorcentajeIva { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ImporteIva { get; set; }
}
