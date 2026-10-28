using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.EstadosCompra;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IEstadoCompraService
{
    Task<BaseResponseDto<IEnumerable<EstadoCompraResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EstadoCompraResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EstadoCompraResponseDto>> CrearAsync(CrearEstadoCompraRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EstadoCompraResponseDto>> ActualizarAsync(ActualizarEstadoCompraRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
