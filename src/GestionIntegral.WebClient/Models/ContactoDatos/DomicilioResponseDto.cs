namespace GestionIntegral.WebClient.Models.ContactoDatos;

public class DomicilioResponseDto
{
    public int IdDomicilio { get; set; }
    public string TipoReferencia { get; set; } = string.Empty;
    public int IdReferencia { get; set; }
    public string Domicilio { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string? Localidad { get; set; }
    public int IdProvincia { get; set; }
    public string? CodigoPostal { get; set; }
    public bool Activo { get; set; }
    public string? ProvinciaDescripcion { get; set; }
}
