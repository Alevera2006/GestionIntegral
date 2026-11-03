namespace GestionIntegral.WebClient.Models.Stock;

public class ProductoLookupDto
{
    public int IdProducto { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int IdCategoria { get; set; }
    public int? IdSubCategoria { get; set; }
}
