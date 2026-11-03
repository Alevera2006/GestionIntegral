using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.PagosProveedores;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IPagoProveedorService
{
    Task<BaseResponseDto<IEnumerable<PagoProveedorResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<IEnumerable<PagoProveedorResponseDto>>> ObtenerPorProveedorAsync(int idProveedor, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<PagoProveedorResponseDto>> CrearAsync(PagoProveedorRequestDto request, CancellationToken cancellationToken = default);
}
