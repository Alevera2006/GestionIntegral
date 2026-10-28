namespace GestionIntegral.Domain.Entities;

public class MotivoAnulacion : ConfiguracionBase
{
    public int IdMotivoAnulacion { get; set; }

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }
}
