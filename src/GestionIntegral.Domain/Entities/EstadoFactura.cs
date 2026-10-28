namespace GestionIntegral.Domain.Entities;

public class EstadoFactura : ConfiguracionBase
{
    public int IdEstadoFactura { get; set; }

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }
}
