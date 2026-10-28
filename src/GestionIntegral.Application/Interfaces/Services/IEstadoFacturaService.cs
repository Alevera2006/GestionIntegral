using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.EstadosFactura;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IEstadoFacturaService
{
    Task<BaseResponseDto<IEnumerable<EstadoFacturaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EstadoFacturaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EstadoFacturaResponseDto>> CrearAsync(CrearEstadoFacturaRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EstadoFacturaResponseDto>> ActualizarAsync(ActualizarEstadoFacturaRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
