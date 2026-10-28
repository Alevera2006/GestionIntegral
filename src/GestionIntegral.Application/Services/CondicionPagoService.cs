using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.CondicionesPago;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class CondicionPagoService : ICondicionPagoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CondicionPagoService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<CondicionPagoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var condiciones = await _unitOfWork.CondicionesPago.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<CondicionPagoResponseDto>>.Ok(_mapper.Map<IEnumerable<CondicionPagoResponseDto>>(condiciones));
    }

    public async Task<BaseResponseDto<CondicionPagoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var condicion = await _unitOfWork.CondicionesPago.GetByIdAsync(id, cancellationToken);
        if (condicion is null)
            throw new NotFoundException(nameof(CondicionPago), id);

        return BaseResponseDto<CondicionPagoResponseDto>.Ok(_mapper.Map<CondicionPagoResponseDto>(condicion));
    }

    public async Task<BaseResponseDto<CondicionPagoResponseDto>> CrearAsync(CrearCondicionPagoRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.CondicionesPago.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe una condición de pago con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var condicion = new CondicionPago
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Dias = request.Dias,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.CondicionesPago.AddAsync(condicion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<CondicionPagoResponseDto>.Ok(_mapper.Map<CondicionPagoResponseDto>(condicion), "Condición de pago creada correctamente.");
    }

    public async Task<BaseResponseDto<CondicionPagoResponseDto>> ActualizarAsync(ActualizarCondicionPagoRequestDto request, CancellationToken cancellationToken = default)
    {
        var condicion = await _unitOfWork.CondicionesPago.GetByIdAsync(request.IdCondicionPago, cancellationToken);
        if (condicion is null)
            throw new NotFoundException(nameof(CondicionPago), request.IdCondicionPago);

        var existente = await _unitOfWork.CondicionesPago.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdCondicionPago != request.IdCondicionPago)
            throw new BusinessException($"Ya existe una condición de pago con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        condicion.Codigo = request.Codigo;
        condicion.Descripcion = request.Descripcion;
        condicion.Dias = request.Dias;
        condicion.Activo = request.Activo;
        condicion.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        condicion.IdUsuarioUltimaModificacion = userId;
        condicion.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.CondicionesPago.UpdateAsync(condicion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<CondicionPagoResponseDto>.Ok(_mapper.Map<CondicionPagoResponseDto>(condicion), "Condición de pago actualizada correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var condicion = await _unitOfWork.CondicionesPago.GetByIdAsync(id, cancellationToken);
        if (condicion is null)
            throw new NotFoundException(nameof(CondicionPago), id);

        if (!condicion.Activo)
            throw new BusinessException("La condición de pago ya se encuentra dada de baja.");

        var userId = _currentUserService.UserId ?? 1;

        condicion.Activo = false;
        condicion.FechaBaja = DateTime.UtcNow;
        condicion.IdUsuarioUltimaModificacion = userId;
        condicion.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.CondicionesPago.UpdateAsync(condicion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Condición de pago dada de baja correctamente.");
    }
}
