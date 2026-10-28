namespace GestionIntegral.Domain.Entities;

public class AlicuotaImpuesto : EntityBase
{
    public int IdAlicuotaImpuesto { get; set; }
    public int IdTipoImpuesto { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Porcentaje { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime? FechaBaja { get; set; }

    public virtual TipoImpuesto TipoImpuesto { get; set; } = null!;
    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }
}
