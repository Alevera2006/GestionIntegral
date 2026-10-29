using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.ClienteContactos;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IClienteContactoService
{
    Task<BaseResponseDto<IEnumerable<ClienteContactoResponseDto>>> ObtenerPorClienteAsync(int idCliente, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ClienteContactoResponseDto>> CrearAsync(CrearClienteContactoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int idCliente, int idContacto, CancellationToken cancellationToken = default);
}
