using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Cajas;

namespace GestionIntegral.Application.Interfaces.Services;

public interface ICajaCabeceraService
{
    Task<BaseResponseDto<IEnumerable<CajaCabeceraResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<CajaCabeceraResponseDto>> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<CajaCabeceraResponseDto>> CrearAsync(CrearCajaCabeceraRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<CajaCabeceraResponseDto>> ActualizarAsync(ActualizarCajaCabeceraRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(long id, CancellationToken cancellationToken = default);
}
