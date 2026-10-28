namespace GestionIntegral.Application.DTOs.Cajas;

public class CajaCabeceraResponseDto
{
    public long IdCaja { get; set; }
    public DateTime FechaCaja { get; set; }
    public int IdUsuarioResponsable { get; set; }
    public string NombreUsuarioResponsable { get; set; } = string.Empty;
    public int IdEstadoCaja { get; set; }
    public string EstadoCajaDescripcion { get; set; } = string.Empty;
    public decimal SaldoInicial { get; set; }
    public decimal? SaldoFinal { get; set; }
    public DateTime FechaApertura { get; set; }
    public DateTime? FechaCierre { get; set; }
    public string? Observaciones { get; set; }
}
