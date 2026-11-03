namespace GestionIntegral.WebClient.Models.CuentasCorrientes;

public class CuentaCorrienteClienteDto
{
    public int IdCliente { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;
    public decimal Saldo { get; set; }
}

public class MovimientoCuentaCorrienteDto
{
    public long IdMovimiento { get; set; }
    public DateTime Fecha { get; set; }
    public string Tipo { get; set; } = string.Empty; // Factura, Pago, NotaCredito, NotaDebito
    public string Concepto { get; set; } = string.Empty;
    public string NumeroComprobante { get; set; } = string.Empty;
    public decimal Debe { get; set; }
    public decimal Haber { get; set; }
    public decimal Saldo { get; set; }
}

public class PagoClienteRequestDto
{
    public int IdCliente { get; set; }
    public long? IdFactura { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Today;
    public decimal Monto { get; set; }
    public int IdMedioPago { get; set; }
    public string? NumeroCheque { get; set; }
    public string? Banco { get; set; }
    public DateTime? FechaVencimientoCheque { get; set; }
    public string Observaciones { get; set; } = string.Empty;
}
