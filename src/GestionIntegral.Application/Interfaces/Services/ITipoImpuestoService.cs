using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.TiposImpuesto;

namespace GestionIntegral.Application.Interfaces.Services;

public interface ITipoImpuestoService
{
    Task<BaseResponseDto<IEnumerable<TipoImpuestoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TipoImpuestoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TipoImpuestoResponseDto>> CrearAsync(CrearTipoImpuestoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TipoImpuestoResponseDto>> ActualizarAsync(ActualizarTipoImpuestoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
