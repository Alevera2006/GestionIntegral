namespace GestionIntegral.Domain.Entities;

public class MovimientoStock : EntityBase
{
    public long IdMovimientoStock { get; set; }
    public int IdDeposito { get; set; }
    public int IdTipoMovimientoStock { get; set; }
    public DateTime FechaMovimiento { get; set; }
    public int IdUsuarioResponsable { get; set; }
    public string? Referencia { get; set; }
    public string? Observaciones { get; set; }

    public virtual Deposito Deposito { get; set; } = null!;
    public virtual TipoMovimientoStock TipoMovimientoStock { get; set; } = null!;
    public virtual Usuario UsuarioResponsable { get; set; } = null!;
    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual ICollection<MovimientoStockDetalle> Detalles { get; set; } = new List<MovimientoStockDetalle>();
}
