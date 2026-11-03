using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.CuentasCorrientes;

namespace GestionIntegral.Application.Interfaces.Services;

public interface ICuentaCorrienteService
{
    Task<BaseResponseDto<IEnumerable<CuentaCorrienteClienteDto>>> ObtenerSaldosClientesAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<IEnumerable<MovimientoCuentaCorrienteDto>>> ObtenerMovimientosAsync(int idCliente, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> RegistrarPagoAsync(PagoClienteRequestDto request, CancellationToken cancellationToken = default);
}
