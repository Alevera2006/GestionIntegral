using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Domicilios;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IDomicilioService
{
    Task<BaseResponseDto<IEnumerable<DomicilioResponseDto>>> ObtenerTodosAsync(string? tipoReferencia, int? idReferencia, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<DomicilioResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<DomicilioResponseDto>> CrearAsync(CrearDomicilioRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<DomicilioResponseDto>> ActualizarAsync(ActualizarDomicilioRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
