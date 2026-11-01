using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Emails;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IEmailService
{
    Task<BaseResponseDto<IEnumerable<EmailResponseDto>>> ObtenerTodosAsync(string? tipoReferencia, int? idReferencia, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EmailResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EmailResponseDto>> CrearAsync(CrearEmailRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<EmailResponseDto>> ActualizarAsync(ActualizarEmailRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
