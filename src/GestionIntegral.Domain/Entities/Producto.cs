namespace GestionIntegral.Domain.Entities;

public class Producto : EntityBase
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
    public bool Activo { get; set; } = true;
    public DateTime? FechaBaja { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;
    public virtual SubCategoria? SubCategoria { get; set; }
    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }

    public virtual ICollection<ListaPrecioDetalle> ListaPrecioDetalles { get; set; } = new List<ListaPrecioDetalle>();
}
