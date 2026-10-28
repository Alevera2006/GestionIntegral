namespace GestionIntegral.Application.DTOs.TiposImpuesto;

public class TipoImpuestoResponseDto
{
    public int IdTipoImpuesto { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
