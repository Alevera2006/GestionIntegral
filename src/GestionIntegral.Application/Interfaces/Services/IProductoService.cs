using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Productos;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IProductoService
{
    Task<BaseResponseDto<IEnumerable<ProductoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ProductoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ProductoResponseDto>> CrearAsync(CrearProductoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ProductoResponseDto>> ActualizarAsync(ActualizarProductoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
