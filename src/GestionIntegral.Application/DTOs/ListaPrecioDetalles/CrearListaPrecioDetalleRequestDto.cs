namespace GestionIntegral.Application.DTOs.ListaPrecioDetalles;

public class CrearListaPrecioDetalleRequestDto
{
    public int IdListaPrecio { get; set; }
    public int IdProducto { get; set; }
    public decimal Precio { get; set; }
}
