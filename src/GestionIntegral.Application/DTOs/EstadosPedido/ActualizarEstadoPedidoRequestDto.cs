using GestionIntegral.Application.DTOs.ConfiguracionBase;

namespace GestionIntegral.Application.DTOs.EstadosPedido;

public class ActualizarEstadoPedidoRequestDto : ActualizarConfiguracionRequestDto
{
    public int IdEstadoPedido { get; set; }
}
