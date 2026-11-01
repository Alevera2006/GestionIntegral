namespace GestionIntegral.WebClient.Models.ContactoDatos;

public class CrearTelefonoRequestDto
{
    public string TipoReferencia { get; set; } = string.Empty;
    public int IdReferencia { get; set; }
    public int? IdCliente { get; set; }
    public int? IdProveedor { get; set; }
    public string NumeroTelefono { get; set; } = string.Empty;
    public string TipoTelefono { get; set; } = string.Empty;
}
