namespace GestionIntegral.WebClient.Models.Clientes;

public class CrearClienteRequestDto
{
    public string Codigo { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string? NombreFantasia { get; set; }
    public string? Cuit { get; set; }
    public int? IdCondicionPago { get; set; }
    public string? Observaciones { get; set; }
}
