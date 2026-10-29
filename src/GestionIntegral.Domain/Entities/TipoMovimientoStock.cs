namespace GestionIntegral.Domain.Entities;

public class TipoMovimientoStock : EntityBase
{
    public int IdTipoMovimientoStock { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public short Signo { get; set; }
    public bool Activo { get; set; } = true;

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual ICollection<MovimientoStock> MovimientosStock { get; set; } = new List<MovimientoStock>();
}
