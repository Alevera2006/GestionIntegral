namespace GestionIntegral.Domain.Entities;

public class Contacto : EntityBase
{
    public int IdContacto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Apellido { get; set; }
    public string? Cargo { get; set; }
    public string? Telefono { get; set; }
    public string? Celular { get; set; }
    public string? Email { get; set; }
    public bool Activo { get; set; } = true;

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual ICollection<ClienteContacto> ClienteContactos { get; set; } = new List<ClienteContacto>();
}
