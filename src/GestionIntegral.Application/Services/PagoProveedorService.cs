using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.PagosProveedores;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class PagoProveedorService : IPagoProveedorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public PagoProveedorService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<PagoProveedorResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var pagos = await _unitOfWork.PagosProveedores.GetAllWithDetailsAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<PagoProveedorResponseDto>>.Ok(_mapper.Map<IEnumerable<PagoProveedorResponseDto>>(pagos));
    }

    public async Task<BaseResponseDto<IEnumerable<PagoProveedorResponseDto>>> ObtenerPorProveedorAsync(int idProveedor, CancellationToken cancellationToken = default)
    {
        var proveedor = await _unitOfWork.Proveedores.GetByIdAsync(idProveedor, cancellationToken);
        if (proveedor is null)
            throw new NotFoundException(nameof(Proveedor), idProveedor);

        var pagos = await _unitOfWork.PagosProveedores.GetByProveedorAsync(idProveedor, cancellationToken);
        return BaseResponseDto<IEnumerable<PagoProveedorResponseDto>>.Ok(_mapper.Map<IEnumerable<PagoProveedorResponseDto>>(pagos));
    }

    public async Task<BaseResponseDto<PagoProveedorResponseDto>> CrearAsync(PagoProveedorRequestDto request, CancellationToken cancellationToken = default)
    {
        var proveedor = await _unitOfWork.Proveedores.GetByIdAsync(request.IdProveedor, cancellationToken);
        if (proveedor is null)
            throw new NotFoundException(nameof(Proveedor), request.IdProveedor);

        var medioPago = await _unitOfWork.MediosPago.GetByIdAsync(request.IdMedioPago, cancellationToken);
        if (medioPago is null)
            throw new NotFoundException(nameof(MedioPago), request.IdMedioPago);

        var pago = new PagoProveedor
        {
            IdProveedor = request.IdProveedor,
            Fecha = request.Fecha,
            Monto = request.Monto,
            IdMedioPago = request.IdMedioPago,
            NumeroCheque = request.NumeroCheque,
            Banco = request.Banco,
            FechaVencimientoCheque = request.FechaVencimientoCheque,
            Concepto = request.Concepto ?? string.Empty,
            Observaciones = request.Observaciones ?? string.Empty,
            Activo = true,
            IdUsuarioCreacion = _currentUserService.UserId ?? 1,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.PagosProveedores.AddAsync(pago, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var pagoConDetalle = (await _unitOfWork.PagosProveedores.GetByProveedorAsync(request.IdProveedor, cancellationToken))
            .FirstOrDefault(p => p.IdPagoProveedor == pago.IdPagoProveedor);

        return BaseResponseDto<PagoProveedorResponseDto>.Ok(_mapper.Map<PagoProveedorResponseDto>(pagoConDetalle ?? pago), "Pago registrado correctamente.");
    }
}
