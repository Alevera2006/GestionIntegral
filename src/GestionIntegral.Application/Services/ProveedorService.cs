using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Proveedores;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class ProveedorService : IProveedorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public ProveedorService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<ProveedorResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var proveedores = await _unitOfWork.Proveedores.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<ProveedorResponseDto>>.Ok(_mapper.Map<IEnumerable<ProveedorResponseDto>>(proveedores));
    }

    public async Task<BaseResponseDto<ProveedorResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var proveedor = await _unitOfWork.Proveedores.GetByIdAsync(id, cancellationToken);
        if (proveedor is null)
            throw new NotFoundException(nameof(Proveedor), id);

        return BaseResponseDto<ProveedorResponseDto>.Ok(_mapper.Map<ProveedorResponseDto>(proveedor));
    }

    public async Task<BaseResponseDto<ProveedorResponseDto>> CrearAsync(CrearProveedorRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Proveedores.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe un proveedor con el código '{request.Codigo}'.");

        if (request.IdCondicionPago.HasValue)
        {
            var condicion = await _unitOfWork.CondicionesPago.GetByIdAsync(request.IdCondicionPago.Value, cancellationToken);
            if (condicion is null)
                throw new NotFoundException(nameof(CondicionPago), request.IdCondicionPago.Value);
        }

        var userId = _currentUserService.UserId ?? 1;

        var proveedor = new Proveedor
        {
            Codigo = request.Codigo,
            RazonSocial = request.RazonSocial,
            NombreFantasia = request.NombreFantasia,
            CUIT = request.CUIT,
            Email = request.Email,
            Telefono = request.Telefono,
            IdCondicionPago = request.IdCondicionPago,
            Observaciones = request.Observaciones,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Proveedores.AddAsync(proveedor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<ProveedorResponseDto>.Ok(_mapper.Map<ProveedorResponseDto>(proveedor), "Proveedor creado correctamente.");
    }

    public async Task<BaseResponseDto<ProveedorResponseDto>> ActualizarAsync(ActualizarProveedorRequestDto request, CancellationToken cancellationToken = default)
    {
        var proveedor = await _unitOfWork.Proveedores.GetByIdAsync(request.IdProveedor, cancellationToken);
        if (proveedor is null)
            throw new NotFoundException(nameof(Proveedor), request.IdProveedor);

        var existente = await _unitOfWork.Proveedores.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdProveedor != request.IdProveedor)
            throw new BusinessException($"Ya existe un proveedor con el código '{request.Codigo}'.");

        if (request.IdCondicionPago.HasValue)
        {
            var condicion = await _unitOfWork.CondicionesPago.GetByIdAsync(request.IdCondicionPago.Value, cancellationToken);
            if (condicion is null)
                throw new NotFoundException(nameof(CondicionPago), request.IdCondicionPago.Value);
        }

        var userId = _currentUserService.UserId ?? 1;

        proveedor.Codigo = request.Codigo;
        proveedor.RazonSocial = request.RazonSocial;
        proveedor.NombreFantasia = request.NombreFantasia;
        proveedor.CUIT = request.CUIT;
        proveedor.Email = request.Email;
        proveedor.Telefono = request.Telefono;
        proveedor.IdCondicionPago = request.IdCondicionPago;
        proveedor.Observaciones = request.Observaciones;
        proveedor.Activo = request.Activo;
        proveedor.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        proveedor.IdUsuarioUltimaModificacion = userId;
        proveedor.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Proveedores.UpdateAsync(proveedor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<ProveedorResponseDto>.Ok(_mapper.Map<ProveedorResponseDto>(proveedor), "Proveedor actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var proveedor = await _unitOfWork.Proveedores.GetByIdAsync(id, cancellationToken);
        if (proveedor is null)
            throw new NotFoundException(nameof(Proveedor), id);

        if (!proveedor.Activo)
            throw new BusinessException("El proveedor ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        proveedor.Activo = false;
        proveedor.FechaBaja = DateTime.UtcNow;
        proveedor.IdUsuarioUltimaModificacion = userId;
        proveedor.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Proveedores.UpdateAsync(proveedor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Proveedor dado de baja correctamente.");
    }
}
