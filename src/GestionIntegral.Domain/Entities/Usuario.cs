namespace GestionIntegral.Domain.Entities;

public class Usuario : EntityBase
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public int IdTipoUsuario { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public string ClaveHash { get; set; } = string.Empty;
    public bool Bloqueado { get; set; }
    public bool Activo { get; set; }
    public DateTime? FechaBaja { get; set; }

    public virtual TipoUsuario TipoUsuario { get; set; } = null!;
    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }

    public virtual ICollection<Usuario> UsuariosCreados { get; set; } = new List<Usuario>();
    public virtual ICollection<Usuario> UsuariosModificados { get; set; } = new List<Usuario>();
    public virtual ICollection<TipoUsuario> TiposUsuarioCreados { get; set; } = new List<TipoUsuario>();
    public virtual ICollection<TipoUsuario> TiposUsuarioModificados { get; set; } = new List<TipoUsuario>();
}
