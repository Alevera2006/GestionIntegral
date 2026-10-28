namespace GestionIntegral.Domain.Entities;

public class CondicionPago : EntityBase
{
    public int IdCondicionPago { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int Dias { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime? FechaBaja { get; set; }

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    public virtual ICollection<Proveedor> Proveedores { get; set; } = new List<Proveedor>();
}
