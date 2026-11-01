namespace GestionIntegral.Domain.Entities;

public class Telefono : EntityBase
{
    public int IdTelefono { get; set; }
    public string TipoReferencia { get; set; } = string.Empty;
    public int IdReferencia { get; set; }
    public string NumeroTelefono { get; set; } = string.Empty;
    public string TipoTelefono { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public DateTime? FechaBaja { get; set; }

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }
}
