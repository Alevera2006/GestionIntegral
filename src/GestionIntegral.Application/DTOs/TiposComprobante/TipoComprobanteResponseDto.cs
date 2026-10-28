using GestionIntegral.Application.DTOs.ConfiguracionBase;

namespace GestionIntegral.Application.DTOs.TiposComprobante;

public class TipoComprobanteResponseDto : ConfiguracionResponseDto
{
    public int IdTipoComprobante { get; set; }
    public string? CodigoAFIP { get; set; }
}
