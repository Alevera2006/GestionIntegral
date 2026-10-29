namespace GestionIntegral.Application.DTOs.TiposMovimientoStock;

public class ActualizarTipoMovimientoStockRequestDto
{
    public int IdTipoMovimientoStock { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public short Signo { get; set; }
    public bool Activo { get; set; }
}
