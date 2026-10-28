using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.TiposMovimientoCaja;

namespace GestionIntegral.Application.Interfaces.Services;

public interface ITipoMovimientoCajaService
{
    Task<BaseResponseDto<IEnumerable<TipoMovimientoCajaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TipoMovimientoCajaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TipoMovimientoCajaResponseDto>> CrearAsync(CrearTipoMovimientoCajaRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TipoMovimientoCajaResponseDto>> ActualizarAsync(ActualizarTipoMovimientoCajaRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
