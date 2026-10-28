using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.EstadosPedido;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IEstadoPedidoService
{
    Task<BaseResponseDto<IEnumerable<EstadoPedidoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EstadoPedidoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EstadoPedidoResponseDto>> CrearAsync(CrearEstadoPedidoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EstadoPedidoResponseDto>> ActualizarAsync(ActualizarEstadoPedidoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
