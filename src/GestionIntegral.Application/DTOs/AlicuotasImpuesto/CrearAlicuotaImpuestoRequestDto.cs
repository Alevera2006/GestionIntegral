namespace GestionIntegral.Application.DTOs.AlicuotasImpuesto;

public class CrearAlicuotaImpuestoRequestDto
{
    public int IdTipoImpuesto { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Porcentaje { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
}
