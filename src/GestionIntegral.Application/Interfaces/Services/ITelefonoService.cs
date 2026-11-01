using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Telefonos;

namespace GestionIntegral.Application.Interfaces.Services;

public interface ITelefonoService
{
    Task<BaseResponseDto<IEnumerable<TelefonoResponseDto>>> ObtenerTodosAsync(string? tipoReferencia, int? idReferencia, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TelefonoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TelefonoResponseDto>> CrearAsync(CrearTelefonoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TelefonoResponseDto>> ActualizarAsync(ActualizarTelefonoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
