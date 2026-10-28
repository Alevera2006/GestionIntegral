using GestionIntegral.Application.DTOs.ConfiguracionBase;

namespace GestionIntegral.Application.DTOs.EstadosProceso;

public class ActualizarEstadoProcesoRequestDto : ActualizarConfiguracionRequestDto
{
    public int IdEstadoProceso { get; set; }
}
