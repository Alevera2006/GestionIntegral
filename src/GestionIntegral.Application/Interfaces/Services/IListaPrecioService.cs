using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.ListaPrecioDetalles;
using GestionIntegral.Application.DTOs.ListasPrecios;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IListaPrecioService
{
    Task<BaseResponseDto<IEnumerable<ListaPrecioResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ListaPrecioResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ListaPrecioResponseDto>> CrearAsync(CrearListaPrecioRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ListaPrecioResponseDto>> ActualizarAsync(ActualizarListaPrecioRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);

    Task<BaseResponseDto<IEnumerable<ListaPrecioDetalleResponseDto>>> ObtenerDetallesAsync(int idListaPrecio, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ListaPrecioDetalleResponseDto>> AgregarDetalleAsync(CrearListaPrecioDetalleRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ListaPrecioDetalleResponseDto>> ActualizarDetalleAsync(ActualizarListaPrecioDetalleRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarDetalleAsync(int idListaPrecioDetalle, CancellationToken cancellationToken = default);
}
