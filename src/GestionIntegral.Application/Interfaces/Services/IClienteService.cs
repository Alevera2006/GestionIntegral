using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Clientes;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IClienteService
{
    Task<BaseResponseDto<IEnumerable<ClienteResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ClienteResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ClienteResponseDto>> CrearAsync(CrearClienteRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ClienteResponseDto>> ActualizarAsync(ActualizarClienteRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
