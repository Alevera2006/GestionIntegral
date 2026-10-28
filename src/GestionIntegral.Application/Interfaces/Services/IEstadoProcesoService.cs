using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.EstadosProceso;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IEstadoProcesoService
{
    Task<BaseResponseDto<IEnumerable<EstadoProcesoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EstadoProcesoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EstadoProcesoResponseDto>> CrearAsync(CrearEstadoProcesoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EstadoProcesoResponseDto>> ActualizarAsync(ActualizarEstadoProcesoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
