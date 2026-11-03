namespace GestionIntegral.Application.DTOs.ValoresCartera;

public class ValorCarteraResponseDto
{
    public long IdValorCartera { get; set; }
    public long IdPagoCliente { get; set; }
    public int IdCliente { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;
    public string TipoValor { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string? Banco { get; set; }
    public DateTime? FechaEmision { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public decimal Importe { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
}
