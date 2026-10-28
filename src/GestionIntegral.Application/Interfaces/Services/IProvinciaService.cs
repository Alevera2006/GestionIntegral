using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Provincias;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IProvinciaService
{
    Task<BaseResponseDto<IEnumerable<ProvinciaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ProvinciaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ProvinciaResponseDto>> CrearAsync(CrearProvinciaRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ProvinciaResponseDto>> ActualizarAsync(ActualizarProvinciaRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
