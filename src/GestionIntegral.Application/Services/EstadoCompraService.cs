using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.EstadosCompra;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class EstadoCompraService : IEstadoCompraService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public EstadoCompraService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<EstadoCompraResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var estados = await _unitOfWork.EstadosCompra.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<EstadoCompraResponseDto>>.Ok(_mapper.Map<IEnumerable<EstadoCompraResponseDto>>(estados));
    }

    public async Task<BaseResponseDto<EstadoCompraResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var estado = await _unitOfWork.EstadosCompra.GetByIdAsync(id, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoCompra), id);

        return BaseResponseDto<EstadoCompraResponseDto>.Ok(_mapper.Map<EstadoCompraResponseDto>(estado));
    }

    public async Task<BaseResponseDto<EstadoCompraResponseDto>> CrearAsync(CrearEstadoCompraRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.EstadosCompra.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe un estado de compra con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var estado = new EstadoCompra
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.EstadosCompra.AddAsync(estado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<EstadoCompraResponseDto>.Ok(_mapper.Map<EstadoCompraResponseDto>(estado), "Estado de compra creado correctamente.");
    }

    public async Task<BaseResponseDto<EstadoCompraResponseDto>> ActualizarAsync(ActualizarEstadoCompraRequestDto request, CancellationToken cancellationToken = default)
    {
        var estado = await _unitOfWork.EstadosCompra.GetByIdAsync(request.IdEstadoCompra, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoCompra), request.IdEstadoCompra);

        var existente = await _unitOfWork.EstadosCompra.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdEstadoCompra != request.IdEstadoCompra)
            throw new BusinessException($"Ya existe un estado de compra con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        estado.Codigo = request.Codigo;
        estado.Descripcion = request.Descripcion;
        estado.Activo = request.Activo;
        estado.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        estado.IdUsuarioUltimaModificacion = userId;
        estado.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.EstadosCompra.UpdateAsync(estado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<EstadoCompraResponseDto>.Ok(_mapper.Map<EstadoCompraResponseDto>(estado), "Estado de compra actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var estado = await _unitOfWork.EstadosCompra.GetByIdAsync(id, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoCompra), id);

        if (!estado.Activo)
            throw new BusinessException("El estado de compra ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        estado.Activo = false;
        estado.FechaBaja = DateTime.UtcNow;
        estado.IdUsuarioUltimaModificacion = userId;
        estado.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.EstadosCompra.UpdateAsync(estado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Estado de compra dado de baja correctamente.");
    }
}
