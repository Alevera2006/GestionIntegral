namespace GestionIntegral.Application.DTOs.ListaPrecioDetalles;

public class ListaPrecioDetalleResponseDto
{
    public int IdListaPrecioDetalle { get; set; }
    public int IdListaPrecio { get; set; }
    public int IdProducto { get; set; }
    public string ProductoCodigo { get; set; } = string.Empty;
    public string ProductoDescripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
}
