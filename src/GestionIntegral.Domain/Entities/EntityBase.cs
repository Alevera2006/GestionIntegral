namespace GestionIntegral.Domain.Entities;

public abstract class EntityBase
{
    public int IdUsuarioCreacion { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int? IdUsuarioUltimaModificacion { get; set; }
    public DateTime? FechaUltimaModificacion { get; set; }
}
