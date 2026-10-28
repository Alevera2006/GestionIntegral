using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.MediosPago;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class MedioPagoService : IMedioPagoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public MedioPagoService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<MedioPagoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var medios = await _unitOfWork.MediosPago.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<MedioPagoResponseDto>>.Ok(_mapper.Map<IEnumerable<MedioPagoResponseDto>>(medios));
    }

    public async Task<BaseResponseDto<MedioPagoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var medio = await _unitOfWork.MediosPago.GetByIdAsync(id, cancellationToken);
        if (medio is null)
            throw new NotFoundException(nameof(MedioPago), id);

        return BaseResponseDto<MedioPagoResponseDto>.Ok(_mapper.Map<MedioPagoResponseDto>(medio));
    }

    public async Task<BaseResponseDto<MedioPagoResponseDto>> CrearAsync(CrearMedioPagoRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.MediosPago.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe un medio de pago con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var medio = new MedioPago
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            EsEfectivo = request.EsEfectivo,
            RequiereReferencia = request.RequiereReferencia,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.MediosPago.AddAsync(medio, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<MedioPagoResponseDto>.Ok(_mapper.Map<MedioPagoResponseDto>(medio), "Medio de pago creado correctamente.");
    }

    public async Task<BaseResponseDto<MedioPagoResponseDto>> ActualizarAsync(ActualizarMedioPagoRequestDto request, CancellationToken cancellationToken = default)
    {
        var medio = await _unitOfWork.MediosPago.GetByIdAsync(request.IdMedioPago, cancellationToken);
        if (medio is null)
            throw new NotFoundException(nameof(MedioPago), request.IdMedioPago);

        var existente = await _unitOfWork.MediosPago.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdMedioPago != request.IdMedioPago)
            throw new BusinessException($"Ya existe un medio de pago con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        medio.Codigo = request.Codigo;
        medio.Descripcion = request.Descripcion;
        medio.EsEfectivo = request.EsEfectivo;
        medio.RequiereReferencia = request.RequiereReferencia;
        medio.Activo = request.Activo;
        medio.IdUsuarioUltimaModificacion = userId;
        medio.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.MediosPago.UpdateAsync(medio, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<MedioPagoResponseDto>.Ok(_mapper.Map<MedioPagoResponseDto>(medio), "Medio de pago actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var medio = await _unitOfWork.MediosPago.GetByIdAsync(id, cancellationToken);
        if (medio is null)
            throw new NotFoundException(nameof(MedioPago), id);

        if (!medio.Activo)
            throw new BusinessException("El medio de pago ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        medio.Activo = false;
        medio.IdUsuarioUltimaModificacion = userId;
        medio.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.MediosPago.UpdateAsync(medio, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Medio de pago dado de baja correctamente.");
    }
}
