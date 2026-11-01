namespace GestionIntegral.WebClient.Models.ContactoDatos;

public class CondicionPagoResponseDto
{
    public int IdCondicionPago { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int Dias { get; set; }
    public bool Activo { get; set; }
}
