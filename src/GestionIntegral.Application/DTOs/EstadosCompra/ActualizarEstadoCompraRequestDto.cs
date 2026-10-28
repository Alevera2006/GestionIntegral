using GestionIntegral.Application.DTOs.ConfiguracionBase;

namespace GestionIntegral.Application.DTOs.EstadosCompra;

public class ActualizarEstadoCompraRequestDto : ActualizarConfiguracionRequestDto
{
    public int IdEstadoCompra { get; set; }
}
