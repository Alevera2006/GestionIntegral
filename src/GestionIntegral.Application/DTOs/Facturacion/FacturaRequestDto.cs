namespace GestionIntegral.Application.DTOs.Facturacion;

public class FacturaRequestDto
{
    public int? IdCliente { get; set; }
    public string NombreCliente { get; set; } = "Consumidor Final";
    public int IdTipoComprobante { get; set; }
    public string NumeroComprobante { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.Today;
    public int IdCondicionPago { get; set; }
    public string Observaciones { get; set; } = string.Empty;
    public List<FacturaDetalleRequestDto> Detalles { get; set; } = new();
}

public class FacturaDetalleRequestDto
{
    public int IdProducto { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal PorcentajeIva { get; set; }
}
