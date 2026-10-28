namespace GestionIntegral.Domain.Entities;

public class TipoUsuario : EntityBase
{
    public int IdTipoUsuario { get; set; }
    public string Descripcion { get; set; } = string.Empty;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }
}
