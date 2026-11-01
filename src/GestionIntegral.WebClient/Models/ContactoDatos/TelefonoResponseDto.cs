namespace GestionIntegral.WebClient.Models.ContactoDatos;

public class TelefonoResponseDto
{
    public int IdTelefono { get; set; }
    public string TipoReferencia { get; set; } = string.Empty;
    public int IdReferencia { get; set; }
    public string NumeroTelefono { get; set; } = string.Empty;
    public string TipoTelefono { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
