using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.EstadosCaja;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IEstadoCajaService
{
    Task<BaseResponseDto<IEnumerable<EstadoCajaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EstadoCajaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EstadoCajaResponseDto>> CrearAsync(CrearEstadoCajaRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EstadoCajaResponseDto>> ActualizarAsync(ActualizarEstadoCajaRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
