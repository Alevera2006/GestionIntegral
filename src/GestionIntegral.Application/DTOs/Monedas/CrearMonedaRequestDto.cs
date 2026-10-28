namespace GestionIntegral.Application.DTOs.Monedas;

public class CrearMonedaRequestDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? Simbolo { get; set; }
    public byte CantidadDecimales { get; set; } = 2;
}
