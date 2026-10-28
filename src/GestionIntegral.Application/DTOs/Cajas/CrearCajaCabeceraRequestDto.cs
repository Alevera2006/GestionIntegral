namespace GestionIntegral.Application.DTOs.Cajas;

public class CrearCajaCabeceraRequestDto
{
    public DateTime FechaCaja { get; set; }
    public int IdUsuarioResponsable { get; set; }
    public int IdEstadoCaja { get; set; }
    public decimal SaldoInicial { get; set; }
    public string? Observaciones { get; set; }
}
