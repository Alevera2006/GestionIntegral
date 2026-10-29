namespace GestionIntegral.Domain.Entities;

public class ClienteContacto
{
    public int IdCliente { get; set; }
    public int IdContacto { get; set; }
    public bool EsPrincipal { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;
    public virtual Contacto Contacto { get; set; } = null!;
}
