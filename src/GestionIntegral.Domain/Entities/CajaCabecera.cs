namespace GestionIntegral.Domain.Entities;

public class CajaCabecera : EntityBase
{
    public long IdCaja { get; set; }
    public DateTime FechaCaja { get; set; }
    public int IdUsuarioResponsable { get; set; }
    public int IdEstadoCaja { get; set; }
    public decimal SaldoInicial { get; set; }
    public decimal? SaldoFinal { get; set; }
    public DateTime FechaApertura { get; set; }
    public DateTime? FechaCierre { get; set; }
    public string? Observaciones { get; set; }

    public virtual Usuario UsuarioResponsable { get; set; } = null!;
    public virtual EstadoCaja EstadoCaja { get; set; } = null!;
    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual ICollection<CajaDetalle> Detalles { get; set; } = new List<CajaDetalle>();
}
