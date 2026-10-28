using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Cajas;

namespace GestionIntegral.Application.Interfaces.Services;

public interface ICajaDetalleService
{
    Task<BaseResponseDto<IEnumerable<CajaDetalleResponseDto>>> ObtenerPorCajaAsync(long idCaja, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<CajaDetalleResponseDto>> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<CajaDetalleResponseDto>> CrearAsync(CrearCajaDetalleRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(long id, CancellationToken cancellationToken = default);
}
