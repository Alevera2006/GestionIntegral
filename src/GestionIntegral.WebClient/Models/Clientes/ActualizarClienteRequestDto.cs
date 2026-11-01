namespace GestionIntegral.WebClient.Models.Clientes;

public class ActualizarClienteRequestDto : CrearClienteRequestDto
{
    public int IdCliente { get; set; }
    public bool Activo { get; set; }
}
