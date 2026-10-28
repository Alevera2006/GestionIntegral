namespace GestionIntegral.Application.DTOs.Clientes;

public class ActualizarClienteRequestDto
{
    public int IdCliente { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string? NombreFantasia { get; set; }
    public string? CUIT { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public int? IdCondicionPago { get; set; }
    public string? Observaciones { get; set; }
    public bool Activo { get; set; }
}
