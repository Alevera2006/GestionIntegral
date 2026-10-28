namespace GestionIntegral.Domain.Entities;

public class CondicionIVA : EntityBase
{
    public int IdCondicionIVA { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public DateTime? FechaBaja { get; set; }

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }
}
