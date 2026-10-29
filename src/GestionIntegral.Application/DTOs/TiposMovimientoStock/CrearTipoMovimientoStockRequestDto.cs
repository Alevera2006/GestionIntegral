namespace GestionIntegral.Application.DTOs.TiposMovimientoStock;

public class CrearTipoMovimientoStockRequestDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public short Signo { get; set; }
}
