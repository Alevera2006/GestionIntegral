namespace GestionIntegral.Application.DTOs.TiposMovimientoCaja;

public class ActualizarTipoMovimientoCajaRequestDto
{
    public int IdTipoMovimientoCaja { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public short Signo { get; set; }
    public bool Activo { get; set; }
}
