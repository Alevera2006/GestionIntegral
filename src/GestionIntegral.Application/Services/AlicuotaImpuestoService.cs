using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.AlicuotasImpuesto;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class AlicuotaImpuestoService : IAlicuotaImpuestoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public AlicuotaImpuestoService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<AlicuotaImpuestoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var alicuotas = await _unitOfWork.AlicuotasImpuesto.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<AlicuotaImpuestoResponseDto>>.Ok(_mapper.Map<IEnumerable<AlicuotaImpuestoResponseDto>>(alicuotas));
    }

    public async Task<BaseResponseDto<IEnumerable<AlicuotaImpuestoResponseDto>>> ObtenerPorTipoImpuestoAsync(int idTipoImpuesto, CancellationToken cancellationToken = default)
    {
        var alicuotas = await _unitOfWork.AlicuotasImpuesto.GetByTipoImpuestoAsync(idTipoImpuesto, cancellationToken);
        return BaseResponseDto<IEnumerable<AlicuotaImpuestoResponseDto>>.Ok(_mapper.Map<IEnumerable<AlicuotaImpuestoResponseDto>>(alicuotas));
    }

    public async Task<BaseResponseDto<AlicuotaImpuestoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var alicuota = await _unitOfWork.AlicuotasImpuesto.GetByIdAsync(id, cancellationToken);
        if (alicuota is null)
            throw new NotFoundException(nameof(AlicuotaImpuesto), id);

        return BaseResponseDto<AlicuotaImpuestoResponseDto>.Ok(_mapper.Map<AlicuotaImpuestoResponseDto>(alicuota));
    }

    public async Task<BaseResponseDto<AlicuotaImpuestoResponseDto>> CrearAsync(CrearAlicuotaImpuestoRequestDto request, CancellationToken cancellationToken = default)
    {
        var tipoImpuesto = await _unitOfWork.TiposImpuesto.GetByIdAsync(request.IdTipoImpuesto, cancellationToken);
        if (tipoImpuesto is null)
            throw new NotFoundException(nameof(TipoImpuesto), request.IdTipoImpuesto);

        if (await _unitOfWork.AlicuotasImpuesto.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe una alícuota con el código '{request.Codigo}'.");

        ValidarFechas(request.FechaDesde, request.FechaHasta);

        var userId = _currentUserService.UserId ?? 1;

        var alicuota = new AlicuotaImpuesto
        {
            IdTipoImpuesto = request.IdTipoImpuesto,
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Porcentaje = request.Porcentaje,
            FechaDesde = request.FechaDesde,
            FechaHasta = request.FechaHasta,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.AlicuotasImpuesto.AddAsync(alicuota, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<AlicuotaImpuestoResponseDto>.Ok(_mapper.Map<AlicuotaImpuestoResponseDto>(alicuota), "Alícuota creada correctamente.");
    }

    public async Task<BaseResponseDto<AlicuotaImpuestoResponseDto>> ActualizarAsync(ActualizarAlicuotaImpuestoRequestDto request, CancellationToken cancellationToken = default)
    {
        var alicuota = await _unitOfWork.AlicuotasImpuesto.GetByIdAsync(request.IdAlicuotaImpuesto, cancellationToken);
        if (alicuota is null)
            throw new NotFoundException(nameof(AlicuotaImpuesto), request.IdAlicuotaImpuesto);

        var tipoImpuesto = await _unitOfWork.TiposImpuesto.GetByIdAsync(request.IdTipoImpuesto, cancellationToken);
        if (tipoImpuesto is null)
            throw new NotFoundException(nameof(TipoImpuesto), request.IdTipoImpuesto);

        var existente = await _unitOfWork.AlicuotasImpuesto.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdAlicuotaImpuesto != request.IdAlicuotaImpuesto)
            throw new BusinessException($"Ya existe una alícuota con el código '{request.Codigo}'.");

        ValidarFechas(request.FechaDesde, request.FechaHasta);

        var userId = _currentUserService.UserId ?? 1;

        alicuota.IdTipoImpuesto = request.IdTipoImpuesto;
        alicuota.Codigo = request.Codigo;
        alicuota.Descripcion = request.Descripcion;
        alicuota.Porcentaje = request.Porcentaje;
        alicuota.FechaDesde = request.FechaDesde;
        alicuota.FechaHasta = request.FechaHasta;
        alicuota.Activo = request.Activo;
        alicuota.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        alicuota.IdUsuarioUltimaModificacion = userId;
        alicuota.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.AlicuotasImpuesto.UpdateAsync(alicuota, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<AlicuotaImpuestoResponseDto>.Ok(_mapper.Map<AlicuotaImpuestoResponseDto>(alicuota), "Alícuota actualizada correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var alicuota = await _unitOfWork.AlicuotasImpuesto.GetByIdAsync(id, cancellationToken);
        if (alicuota is null)
            throw new NotFoundException(nameof(AlicuotaImpuesto), id);

        if (!alicuota.Activo)
            throw new BusinessException("La alícuota ya se encuentra dada de baja.");

        var userId = _currentUserService.UserId ?? 1;

        alicuota.Activo = false;
        alicuota.FechaBaja = DateTime.UtcNow;
        alicuota.IdUsuarioUltimaModificacion = userId;
        alicuota.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.AlicuotasImpuesto.UpdateAsync(alicuota, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Alícuota dada de baja correctamente.");
    }

    private static void ValidarFechas(DateTime? fechaDesde, DateTime? fechaHasta)
    {
        if (fechaDesde.HasValue && fechaHasta.HasValue && fechaHasta.Value < fechaDesde.Value)
            throw new BusinessException("La fecha hasta no puede ser menor que la fecha desde.");
    }
}
