namespace GestionIntegral.Application.DTOs.Productos;

public class ActualizarProductoRequestDto
{
    public int IdProducto { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int IdCategoria { get; set; }
    public int? IdSubCategoria { get; set; }
    public string? CodigoBarras { get; set; }
    public decimal? Costo { get; set; }
    public decimal? StockMinimo { get; set; }
    public decimal? StockMaximo { get; set; }
    public string? Observaciones { get; set; }
    public bool Activo { get; set; }
}
