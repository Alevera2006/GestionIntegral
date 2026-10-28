namespace GestionIntegral.Application.DTOs.Monedas;

public class MonedaResponseDto
{
    public int IdMoneda { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? Simbolo { get; set; }
    public byte CantidadDecimales { get; set; }
    public bool Activo { get; set; }
}
