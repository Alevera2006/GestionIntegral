using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Contactos;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IContactoService
{
    Task<BaseResponseDto<IEnumerable<ContactoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ContactoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ContactoResponseDto>> CrearAsync(CrearContactoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ContactoResponseDto>> ActualizarAsync(ActualizarContactoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
