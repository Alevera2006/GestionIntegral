namespace GestionIntegral.Domain.Entities;

public class Categoria : EntityBase
{
    public int IdCategoria { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public DateTime? FechaBaja { get; set; }

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }

    public virtual ICollection<SubCategoria> SubCategorias { get; set; } = new List<SubCategoria>();
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
