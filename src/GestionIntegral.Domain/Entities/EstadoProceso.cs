namespace GestionIntegral.Domain.Entities;

public class EstadoProceso : ConfiguracionBase
{
    public int IdEstadoProceso { get; set; }

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }
}
