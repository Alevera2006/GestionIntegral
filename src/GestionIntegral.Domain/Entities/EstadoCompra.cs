namespace GestionIntegral.Domain.Entities;

public class EstadoCompra : ConfiguracionBase
{
    public int IdEstadoCompra { get; set; }

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }
}
