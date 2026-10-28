using GestionIntegral.Application.DTOs.ConfiguracionBase;

namespace GestionIntegral.Application.DTOs.TiposComprobante;

public class CrearTipoComprobanteRequestDto : CrearConfiguracionRequestDto
{
    public string? CodigoAFIP { get; set; }
}
