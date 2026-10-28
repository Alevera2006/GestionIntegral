namespace GestionIntegral.Application.DTOs.CondicionesPago;

public class CrearCondicionPagoRequestDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int Dias { get; set; }
}
