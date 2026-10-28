namespace GestionIntegral.Domain.Entities;

public class ListaPrecioDetalle : EntityBase
{
    public int IdListaPrecioDetalle { get; set; }
    public int IdListaPrecio { get; set; }
    public int IdProducto { get; set; }
    public decimal Precio { get; set; }

    public virtual ListaPrecio ListaPrecio { get; set; } = null!;
    public virtual Producto Producto { get; set; } = null!;
    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }
}
