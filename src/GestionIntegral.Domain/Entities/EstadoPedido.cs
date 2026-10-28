namespace GestionIntegral.Domain.Entities;

public class EstadoPedido : ConfiguracionBase
{
    public int IdEstadoPedido { get; set; }

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }
}
