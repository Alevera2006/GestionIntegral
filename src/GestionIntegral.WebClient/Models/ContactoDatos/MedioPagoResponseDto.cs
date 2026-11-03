namespace GestionIntegral.WebClient.Models.ContactoDatos;

public class MedioPagoResponseDto
{
    public int IdMedioPago { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
