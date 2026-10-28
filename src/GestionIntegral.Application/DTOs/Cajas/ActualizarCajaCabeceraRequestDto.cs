namespace GestionIntegral.Application.DTOs.Cajas;

public class ActualizarCajaCabeceraRequestDto
{
    public long IdCaja { get; set; }
    public int IdUsuarioResponsable { get; set; }
    public int IdEstadoCaja { get; set; }
    public decimal? SaldoFinal { get; set; }
    public DateTime? FechaCierre { get; set; }
    public string? Observaciones { get; set; }
}
