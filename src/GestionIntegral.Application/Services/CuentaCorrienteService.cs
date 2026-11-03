using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.CuentasCorrientes;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class CuentaCorrienteService : ICuentaCorrienteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CuentaCorrienteService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<CuentaCorrienteClienteDto>>> ObtenerSaldosClientesAsync(CancellationToken cancellationToken = default)
    {
        var clientes = await _unitOfWork.Clientes.GetAllAsync(cancellationToken);
        var facturas = await _unitOfWork.Facturas.GetAllWithDetailsAsync(cancellationToken);
        var pagos = await _unitOfWork.PagosClientes.GetByClienteAsync(0, cancellationToken);

        var saldos = clientes
            .Where(c => c.Activo)
            .Select(c =>
            {
                var debe = facturas
                    .Where(f => f.IdCliente == c.IdCliente && f.Activo && f.CondicionPago.Descripcion.ToLower().Contains("cuenta corriente"))
                    .Sum(f => f.Total);
                var haber = pagos
                    .Where(p => p.IdCliente == c.IdCliente && p.Activo)
                    .Sum(p => p.Monto);
                return new CuentaCorrienteClienteDto
                {
                    IdCliente = c.IdCliente,
                    ClienteNombre = c.RazonSocial,
                    Saldo = debe - haber
                };
            })
            .ToList();

        return BaseResponseDto<IEnumerable<CuentaCorrienteClienteDto>>.Ok(saldos);
    }

    public async Task<BaseResponseDto<IEnumerable<MovimientoCuentaCorrienteDto>>> ObtenerMovimientosAsync(int idCliente, CancellationToken cancellationToken = default)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(idCliente, cancellationToken);
        if (cliente is null)
            throw new NotFoundException(nameof(Cliente), idCliente);

        var facturas = (await _unitOfWork.Facturas.GetAllWithDetailsAsync(cancellationToken))
            .Where(f => f.IdCliente == idCliente && f.Activo && f.CondicionPago.Descripcion.ToLower().Contains("cuenta corriente"))
            .OrderBy(f => f.Fecha);

        var pagos = await _unitOfWork.PagosClientes.GetByClienteAsync(idCliente, cancellationToken);

        var movimientos = new List<MovimientoCuentaCorrienteDto>();
        decimal saldo = 0;

        foreach (var f in facturas)
        {
            saldo += f.Total;
            movimientos.Add(new MovimientoCuentaCorrienteDto
            {
                IdMovimiento = f.IdFactura,
                Fecha = f.Fecha,
                Tipo = "Factura",
                Concepto = $"Factura {f.TipoComprobante.Descripcion}",
                NumeroComprobante = f.NumeroComprobante,
                Debe = f.Total,
                Haber = 0,
                Saldo = saldo
            });
        }

        foreach (var p in pagos)
        {
            saldo -= p.Monto;
            movimientos.Add(new MovimientoCuentaCorrienteDto
            {
                IdMovimiento = p.IdPagoCliente,
                Fecha = p.Fecha,
                Tipo = "Pago",
                Concepto = $"Pago - {p.MedioPago.Descripcion}",
                NumeroComprobante = p.NumeroCheque ?? string.Empty,
                Debe = 0,
                Haber = p.Monto,
                Saldo = saldo
            });
        }

        return BaseResponseDto<IEnumerable<MovimientoCuentaCorrienteDto>>.Ok(movimientos.OrderByDescending(m => m.Fecha));
    }

    public async Task<BaseResponseDto<bool>> RegistrarPagoAsync(PagoClienteRequestDto request, CancellationToken cancellationToken = default)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(request.IdCliente, cancellationToken);
        if (cliente is null)
            throw new NotFoundException(nameof(Cliente), request.IdCliente);

        var medioPago = await _unitOfWork.MediosPago.GetByIdAsync(request.IdMedioPago, cancellationToken);
        if (medioPago is null)
            throw new NotFoundException(nameof(MedioPago), request.IdMedioPago);

        Factura? factura = null;
        if (request.IdFactura.HasValue)
        {
            factura = await _unitOfWork.Facturas.GetByIdLongAsync(request.IdFactura.Value, cancellationToken);
            if (factura is null || factura.IdCliente != request.IdCliente)
                throw new BusinessException("La factura no existe o no pertenece al cliente.");
        }

        var pago = new PagoCliente
        {
            IdCliente = request.IdCliente,
            IdFactura = request.IdFactura,
            Fecha = request.Fecha,
            Monto = request.Monto,
            IdMedioPago = request.IdMedioPago,
            NumeroCheque = request.NumeroCheque,
            Banco = request.Banco,
            FechaVencimientoCheque = request.FechaVencimientoCheque,
            Observaciones = request.Observaciones ?? string.Empty,
            Activo = true,
            IdUsuarioCreacion = _currentUserService.UserId ?? 1,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.PagosClientes.AddAsync(pago, cancellationToken);

        if ((medioPago.Descripcion ?? string.Empty).ToLower().Contains("cheque"))
        {
            var valorCartera = new ValorCartera
            {
                IdPagoCliente = pago.IdPagoCliente,
                IdCliente = request.IdCliente,
                TipoValor = "Cheque",
                Numero = request.NumeroCheque ?? string.Empty,
                Banco = request.Banco,
                FechaEmision = request.Fecha,
                FechaVencimiento = request.FechaVencimientoCheque,
                Importe = request.Monto,
                Estado = "Cartera",
                Observaciones = request.Observaciones,
                Activo = true,
                IdUsuarioCreacion = _currentUserService.UserId ?? 1,
                FechaCreacion = DateTime.UtcNow
            };
            await _unitOfWork.ValoresCartera.AddAsync(valorCartera, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Pago registrado correctamente.");
    }
}
