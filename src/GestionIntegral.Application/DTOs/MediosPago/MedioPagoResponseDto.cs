namespace GestionIntegral.Application.DTOs.MediosPago;

public class MedioPagoResponseDto
{
    public int IdMedioPago { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool EsEfectivo { get; set; }
    public bool RequiereReferencia { get; set; }
    public bool Activo { get; set; }
}
