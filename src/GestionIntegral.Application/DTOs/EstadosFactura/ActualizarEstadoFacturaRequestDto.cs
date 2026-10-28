using GestionIntegral.Application.DTOs.ConfiguracionBase;

namespace GestionIntegral.Application.DTOs.EstadosFactura;

public class ActualizarEstadoFacturaRequestDto : ActualizarConfiguracionRequestDto
{
    public int IdEstadoFactura { get; set; }
}
