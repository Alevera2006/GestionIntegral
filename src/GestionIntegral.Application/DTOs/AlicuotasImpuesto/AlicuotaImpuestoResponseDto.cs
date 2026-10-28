namespace GestionIntegral.Application.DTOs.AlicuotasImpuesto;

public class AlicuotaImpuestoResponseDto
{
    public int IdAlicuotaImpuesto { get; set; }
    public int IdTipoImpuesto { get; set; }
    public string TipoImpuestoDescripcion { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Porcentaje { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public bool Activo { get; set; }
}
