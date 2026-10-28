namespace GestionIntegral.Domain.Entities;

public class TipoComprobante : ConfiguracionBase
{
    public int IdTipoComprobante { get; set; }
    public string? CodigoAFIP { get; set; }

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }
}
