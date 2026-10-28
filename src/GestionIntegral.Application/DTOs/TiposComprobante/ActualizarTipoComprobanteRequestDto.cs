using GestionIntegral.Application.DTOs.ConfiguracionBase;

namespace GestionIntegral.Application.DTOs.TiposComprobante;

public class ActualizarTipoComprobanteRequestDto : ActualizarConfiguracionRequestDto
{
    public int IdTipoComprobante { get; set; }
    public string? CodigoAFIP { get; set; }
}
