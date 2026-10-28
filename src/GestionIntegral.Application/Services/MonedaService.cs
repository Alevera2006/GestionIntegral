using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Monedas;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class MonedaService : IMonedaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public MonedaService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<MonedaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var monedas = await _unitOfWork.Monedas.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<MonedaResponseDto>>.Ok(_mapper.Map<IEnumerable<MonedaResponseDto>>(monedas));
    }

    public async Task<BaseResponseDto<MonedaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var moneda = await _unitOfWork.Monedas.GetByIdAsync(id, cancellationToken);
        if (moneda is null)
            throw new NotFoundException(nameof(Moneda), id);

        return BaseResponseDto<MonedaResponseDto>.Ok(_mapper.Map<MonedaResponseDto>(moneda));
    }

    public async Task<BaseResponseDto<MonedaResponseDto>> CrearAsync(CrearMonedaRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Monedas.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe una moneda con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var moneda = new Moneda
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Simbolo = request.Simbolo,
            CantidadDecimales = request.CantidadDecimales,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Monedas.AddAsync(moneda, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<MonedaResponseDto>.Ok(_mapper.Map<MonedaResponseDto>(moneda), "Moneda creada correctamente.");
    }

    public async Task<BaseResponseDto<MonedaResponseDto>> ActualizarAsync(ActualizarMonedaRequestDto request, CancellationToken cancellationToken = default)
    {
        var moneda = await _unitOfWork.Monedas.GetByIdAsync(request.IdMoneda, cancellationToken);
        if (moneda is null)
            throw new NotFoundException(nameof(Moneda), request.IdMoneda);

        var existente = await _unitOfWork.Monedas.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdMoneda != request.IdMoneda)
            throw new BusinessException($"Ya existe una moneda con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        moneda.Codigo = request.Codigo;
        moneda.Descripcion = request.Descripcion;
        moneda.Simbolo = request.Simbolo;
        moneda.CantidadDecimales = request.CantidadDecimales;
        moneda.Activo = request.Activo;
        moneda.IdUsuarioUltimaModificacion = userId;
        moneda.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Monedas.UpdateAsync(moneda, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<MonedaResponseDto>.Ok(_mapper.Map<MonedaResponseDto>(moneda), "Moneda actualizada correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var moneda = await _unitOfWork.Monedas.GetByIdAsync(id, cancellationToken);
        if (moneda is null)
            throw new NotFoundException(nameof(Moneda), id);

        if (!moneda.Activo)
            throw new BusinessException("La moneda ya se encuentra dada de baja.");

        var userId = _currentUserService.UserId ?? 1;

        moneda.Activo = false;
        moneda.IdUsuarioUltimaModificacion = userId;
        moneda.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Monedas.UpdateAsync(moneda, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Moneda dada de baja correctamente.");
    }
}
