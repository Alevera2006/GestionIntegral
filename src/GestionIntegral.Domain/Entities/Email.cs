namespace GestionIntegral.Domain.Entities;

public class Email : EntityBase
{
    public int IdEmail { get; set; }
    public string TipoReferencia { get; set; } = string.Empty;
    public int IdReferencia { get; set; }
    public string DireccionEmail { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public DateTime? FechaBaja { get; set; }

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }
}
