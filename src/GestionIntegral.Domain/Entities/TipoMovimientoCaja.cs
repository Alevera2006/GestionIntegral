namespace GestionIntegral.Domain.Entities;

public class TipoMovimientoCaja : EntityBase
{
    public int IdTipoMovimientoCaja { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public short Signo { get; set; }
    public bool Activo { get; set; } = true;

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual ICollection<CajaDetalle> CajaDetalles { get; set; } = new List<CajaDetalle>();
}
