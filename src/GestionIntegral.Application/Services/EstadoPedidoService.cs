using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.EstadosPedido;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class EstadoPedidoService : IEstadoPedidoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public EstadoPedidoService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<EstadoPedidoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var estados = await _unitOfWork.EstadosPedido.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<EstadoPedidoResponseDto>>.Ok(_mapper.Map<IEnumerable<EstadoPedidoResponseDto>>(estados));
    }

    public async Task<BaseResponseDto<EstadoPedidoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var estado = await _unitOfWork.EstadosPedido.GetByIdAsync(id, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoPedido), id);

        return BaseResponseDto<EstadoPedidoResponseDto>.Ok(_mapper.Map<EstadoPedidoResponseDto>(estado));
    }

    public async Task<BaseResponseDto<EstadoPedidoResponseDto>> CrearAsync(CrearEstadoPedidoRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.EstadosPedido.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe un estado de pedido con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var estado = new EstadoPedido
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.EstadosPedido.AddAsync(estado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<EstadoPedidoResponseDto>.Ok(_mapper.Map<EstadoPedidoResponseDto>(estado), "Estado de pedido creado correctamente.");
    }

    public async Task<BaseResponseDto<EstadoPedidoResponseDto>> ActualizarAsync(ActualizarEstadoPedidoRequestDto request, CancellationToken cancellationToken = default)
    {
        var estado = await _unitOfWork.EstadosPedido.GetByIdAsync(request.IdEstadoPedido, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoPedido), request.IdEstadoPedido);

        var existente = await _unitOfWork.EstadosPedido.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdEstadoPedido != request.IdEstadoPedido)
            throw new BusinessException($"Ya existe un estado de pedido con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        estado.Codigo = request.Codigo;
        estado.Descripcion = request.Descripcion;
        estado.Activo = request.Activo;
        estado.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        estado.IdUsuarioUltimaModificacion = userId;
        estado.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.EstadosPedido.UpdateAsync(estado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<EstadoPedidoResponseDto>.Ok(_mapper.Map<EstadoPedidoResponseDto>(estado), "Estado de pedido actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var estado = await _unitOfWork.EstadosPedido.GetByIdAsync(id, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoPedido), id);

        if (!estado.Activo)
            throw new BusinessException("El estado de pedido ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        estado.Activo = false;
        estado.FechaBaja = DateTime.UtcNow;
        estado.IdUsuarioUltimaModificacion = userId;
        estado.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.EstadosPedido.UpdateAsync(estado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Estado de pedido dado de baja correctamente.");
    }
}
