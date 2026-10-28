namespace GestionIntegral.Application.DTOs.EstadosCaja;

public class EstadoCajaResponseDto
{
    public int IdEstadoCaja { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
