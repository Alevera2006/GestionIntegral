namespace GestionIntegral.Domain.Entities;

public class Stock
{
    public long IdStock { get; set; }
    public int IdDeposito { get; set; }
    public int IdProducto { get; set; }
    public decimal Cantidad { get; set; }
    public DateTime? FechaUltimoMovimiento { get; set; }

    public virtual Deposito Deposito { get; set; } = null!;
    public virtual Producto Producto { get; set; } = null!;
}
