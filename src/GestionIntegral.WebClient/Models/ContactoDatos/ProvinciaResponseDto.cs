namespace GestionIntegral.WebClient.Models.ContactoDatos;

public class ProvinciaResponseDto
{
    public int IdProvincia { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? CodigoAfip { get; set; }
    public bool Activo { get; set; }
}
