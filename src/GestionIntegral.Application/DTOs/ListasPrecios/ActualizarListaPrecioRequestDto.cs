namespace GestionIntegral.Application.DTOs.ListasPrecios;

public class ActualizarListaPrecioRequestDto
{
    public int IdListaPrecio { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public bool Activo { get; set; }
}
