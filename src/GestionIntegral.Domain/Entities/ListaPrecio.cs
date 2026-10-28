namespace GestionIntegral.Domain.Entities;

public class ListaPrecio : EntityBase
{
    public int IdListaPrecio { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime? FechaBaja { get; set; }

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }

    public virtual ICollection<ListaPrecioDetalle> Detalles { get; set; } = new List<ListaPrecioDetalle>();
}
