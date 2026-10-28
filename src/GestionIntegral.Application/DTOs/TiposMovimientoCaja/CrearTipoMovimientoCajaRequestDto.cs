namespace GestionIntegral.Application.DTOs.TiposMovimientoCaja;

public class CrearTipoMovimientoCajaRequestDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public short Signo { get; set; }
}
