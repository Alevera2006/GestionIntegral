using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Depositos;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IDepositoService
{
    Task<BaseResponseDto<IEnumerable<DepositoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<DepositoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<DepositoResponseDto>> CrearAsync(CrearDepositoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<DepositoResponseDto>> ActualizarAsync(ActualizarDepositoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
