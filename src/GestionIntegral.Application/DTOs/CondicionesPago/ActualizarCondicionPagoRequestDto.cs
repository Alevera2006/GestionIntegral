namespace GestionIntegral.Application.DTOs.CondicionesPago;

public class ActualizarCondicionPagoRequestDto
{
    public int IdCondicionPago { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int Dias { get; set; }
    public bool Activo { get; set; }
}
