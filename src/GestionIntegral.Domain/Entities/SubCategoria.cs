namespace GestionIntegral.Domain.Entities;

public class SubCategoria : EntityBase
{
    public int IdSubCategoria { get; set; }
    public int IdCategoria { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public DateTime? FechaBaja { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;
    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
