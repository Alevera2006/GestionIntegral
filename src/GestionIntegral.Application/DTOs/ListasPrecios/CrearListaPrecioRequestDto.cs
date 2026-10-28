namespace GestionIntegral.Application.DTOs.ListasPrecios;

public class CrearListaPrecioRequestDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
}
