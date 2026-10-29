using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Depositos;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class DepositoService : IDepositoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public DepositoService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<DepositoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var depositos = await _unitOfWork.Depositos.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<DepositoResponseDto>>.Ok(_mapper.Map<IEnumerable<DepositoResponseDto>>(depositos));
    }

    public async Task<BaseResponseDto<DepositoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var deposito = await _unitOfWork.Depositos.GetByIdAsync(id, cancellationToken);
        if (deposito is null)
            throw new NotFoundException(nameof(Deposito), id);

        return BaseResponseDto<DepositoResponseDto>.Ok(_mapper.Map<DepositoResponseDto>(deposito));
    }

    public async Task<BaseResponseDto<DepositoResponseDto>> CrearAsync(CrearDepositoRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Depositos.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe un depósito con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var deposito = new Deposito
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Ubicacion = request.Ubicacion,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Depositos.AddAsync(deposito, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<DepositoResponseDto>.Ok(_mapper.Map<DepositoResponseDto>(deposito), "Depósito creado correctamente.");
    }

    public async Task<BaseResponseDto<DepositoResponseDto>> ActualizarAsync(ActualizarDepositoRequestDto request, CancellationToken cancellationToken = default)
    {
        var deposito = await _unitOfWork.Depositos.GetByIdAsync(request.IdDeposito, cancellationToken);
        if (deposito is null)
            throw new NotFoundException(nameof(Deposito), request.IdDeposito);

        var existente = await _unitOfWork.Depositos.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdDeposito != request.IdDeposito)
            throw new BusinessException($"Ya existe un depósito con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        deposito.Codigo = request.Codigo;
        deposito.Descripcion = request.Descripcion;
        deposito.Ubicacion = request.Ubicacion;
        deposito.Activo = request.Activo;
        deposito.IdUsuarioUltimaModificacion = userId;
        deposito.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Depositos.UpdateAsync(deposito, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<DepositoResponseDto>.Ok(_mapper.Map<DepositoResponseDto>(deposito), "Depósito actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var deposito = await _unitOfWork.Depositos.GetByIdAsync(id, cancellationToken);
        if (deposito is null)
            throw new NotFoundException(nameof(Deposito), id);

        if (!deposito.Activo)
            throw new BusinessException("El depósito ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        deposito.Activo = false;
        deposito.IdUsuarioUltimaModificacion = userId;
        deposito.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Depositos.UpdateAsync(deposito, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Depósito dado de baja correctamente.");
    }
}
