namespace GestionIntegral.Domain.Entities;

public abstract class ConfiguracionBase : EntityBase
{
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public DateTime? FechaBaja { get; set; }
}
