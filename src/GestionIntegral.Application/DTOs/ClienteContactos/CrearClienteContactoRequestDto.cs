namespace GestionIntegral.Application.DTOs.ClienteContactos;

public class CrearClienteContactoRequestDto
{
    public int IdCliente { get; set; }
    public int IdContacto { get; set; }
    public bool EsPrincipal { get; set; }
}
