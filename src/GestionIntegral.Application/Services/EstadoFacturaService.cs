using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.EstadosFactura;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class EstadoFacturaService : IEstadoFacturaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public EstadoFacturaService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<EstadoFacturaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var estados = await _unitOfWork.EstadosFactura.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<EstadoFacturaResponseDto>>.Ok(_mapper.Map<IEnumerable<EstadoFacturaResponseDto>>(estados));
    }

    public async Task<BaseResponseDto<EstadoFacturaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var estado = await _unitOfWork.EstadosFactura.GetByIdAsync(id, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoFactura), id);

        return BaseResponseDto<EstadoFacturaResponseDto>.Ok(_mapper.Map<EstadoFacturaResponseDto>(estado));
    }

    public async Task<BaseResponseDto<EstadoFacturaResponseDto>> CrearAsync(CrearEstadoFacturaRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.EstadosFactura.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe un estado de factura con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var estado = new EstadoFactura
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.EstadosFactura.AddAsync(estado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<EstadoFacturaResponseDto>.Ok(_mapper.Map<EstadoFacturaResponseDto>(estado), "Estado de factura creado correctamente.");
    }

    public async Task<BaseResponseDto<EstadoFacturaResponseDto>> ActualizarAsync(ActualizarEstadoFacturaRequestDto request, CancellationToken cancellationToken = default)
    {
        var estado = await _unitOfWork.EstadosFactura.GetByIdAsync(request.IdEstadoFactura, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoFactura), request.IdEstadoFactura);

        var existente = await _unitOfWork.EstadosFactura.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdEstadoFactura != request.IdEstadoFactura)
            throw new BusinessException($"Ya existe un estado de factura con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        estado.Codigo = request.Codigo;
        estado.Descripcion = request.Descripcion;
        estado.Activo = request.Activo;
        estado.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        estado.IdUsuarioUltimaModificacion = userId;
        estado.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.EstadosFactura.UpdateAsync(estado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<EstadoFacturaResponseDto>.Ok(_mapper.Map<EstadoFacturaResponseDto>(estado), "Estado de factura actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var estado = await _unitOfWork.EstadosFactura.GetByIdAsync(id, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoFactura), id);

        if (!estado.Activo)
            throw new BusinessException("El estado de factura ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        estado.Activo = false;
        estado.FechaBaja = DateTime.UtcNow;
        estado.IdUsuarioUltimaModificacion = userId;
        estado.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.EstadosFactura.UpdateAsync(estado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Estado de factura dado de baja correctamente.");
    }
}
