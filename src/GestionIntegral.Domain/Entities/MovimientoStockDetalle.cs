namespace GestionIntegral.Domain.Entities;

public class MovimientoStockDetalle : EntityBase
{
    public long IdMovimientoStockDetalle { get; set; }
    public long IdMovimientoStock { get; set; }
    public int IdProducto { get; set; }
    public decimal Cantidad { get; set; }

    public virtual MovimientoStock MovimientoStock { get; set; } = null!;
    public virtual Producto Producto { get; set; } = null!;
    public virtual Usuario UsuarioCreacion { get; set; } = null!;
}
