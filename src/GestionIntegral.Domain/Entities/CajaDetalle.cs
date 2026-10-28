namespace GestionIntegral.Domain.Entities;

public class CajaDetalle : EntityBase
{
    public long IdCajaDetalle { get; set; }
    public long IdCaja { get; set; }
    public int IdTipoMovimientoCaja { get; set; }
    public int? IdMedioPago { get; set; }
    public int? IdMoneda { get; set; }
    public DateTime FechaMovimiento { get; set; }
    public string Concepto { get; set; } = string.Empty;
    public string? Referencia { get; set; }
    public decimal Importe { get; set; }

    public virtual CajaCabecera Caja { get; set; } = null!;
    public virtual TipoMovimientoCaja TipoMovimientoCaja { get; set; } = null!;
    public virtual MedioPago? MedioPago { get; set; }
    public virtual Moneda? Moneda { get; set; }
    public virtual Usuario UsuarioCreacion { get; set; } = null!;
}
