namespace GestionIntegral.Domain.Entities;

public class EstadoCaja : EntityBase
{
    public int IdEstadoCaja { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual ICollection<CajaCabecera> Cajas { get; set; } = new List<CajaCabecera>();
}
