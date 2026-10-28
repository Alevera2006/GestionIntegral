using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.TiposComprobante;

namespace GestionIntegral.Application.Interfaces.Services;

public interface ITipoComprobanteService
{
    Task<BaseResponseDto<IEnumerable<TipoComprobanteResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TipoComprobanteResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TipoComprobanteResponseDto>> CrearAsync(CrearTipoComprobanteRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TipoComprobanteResponseDto>> ActualizarAsync(ActualizarTipoComprobanteRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
