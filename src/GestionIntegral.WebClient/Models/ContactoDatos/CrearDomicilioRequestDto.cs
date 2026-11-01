namespace GestionIntegral.WebClient.Models.ContactoDatos;

public class CrearDomicilioRequestDto
{
    public string TipoReferencia { get; set; } = string.Empty;
    public int IdReferencia { get; set; }
    public int? IdCliente { get; set; }
    public int? IdProveedor { get; set; }
    public string Domicilio { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string? Localidad { get; set; }
    public int IdProvincia { get; set; }
    public string? CodigoPostal { get; set; }
}
