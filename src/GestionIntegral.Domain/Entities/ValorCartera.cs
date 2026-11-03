namespace GestionIntegral.Domain.Entities;

public class ValorCartera : EntityBase
{
    public long IdValorCartera { get; set; }
    public long IdPagoCliente { get; set; }
    public int IdCliente { get; set; }
    public string TipoValor { get; set; } = "Cheque"; // Cheque, etc.
    public string Numero { get; set; } = string.Empty;
    public string? Banco { get; set; }
    public DateTime? FechaEmision { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public decimal Importe { get; set; }
    public string Estado { get; set; } = "Cartera"; // Cartera, Depositado, EntregadoProveedor, Acreditado
    public string? Observaciones { get; set; }
    public bool Activo { get; set; } = true;

    public virtual PagoCliente PagoCliente { get; set; } = null!;
    public virtual Cliente Cliente { get; set; } = null!;
}
