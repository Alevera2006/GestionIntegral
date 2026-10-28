namespace GestionIntegral.Domain.Entities;

public class Provincia : ConfiguracionBase
{
    public int IdProvincia { get; set; }
    public string? CodigoAFIP { get; set; }

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }
}
