using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Provincias;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class ProvinciaService : IProvinciaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public ProvinciaService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<ProvinciaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var provincias = await _unitOfWork.Provincias.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<ProvinciaResponseDto>>.Ok(_mapper.Map<IEnumerable<ProvinciaResponseDto>>(provincias));
    }

    public async Task<BaseResponseDto<ProvinciaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var provincia = await _unitOfWork.Provincias.GetByIdAsync(id, cancellationToken);
        if (provincia is null)
            throw new NotFoundException(nameof(Provincia), id);

        return BaseResponseDto<ProvinciaResponseDto>.Ok(_mapper.Map<ProvinciaResponseDto>(provincia));
    }

    public async Task<BaseResponseDto<ProvinciaResponseDto>> CrearAsync(CrearProvinciaRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Provincias.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe una provincia con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var provincia = new Provincia
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            CodigoAFIP = request.CodigoAFIP,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Provincias.AddAsync(provincia, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<ProvinciaResponseDto>.Ok(_mapper.Map<ProvinciaResponseDto>(provincia), "Provincia creada correctamente.");
    }

    public async Task<BaseResponseDto<ProvinciaResponseDto>> ActualizarAsync(ActualizarProvinciaRequestDto request, CancellationToken cancellationToken = default)
    {
        var provincia = await _unitOfWork.Provincias.GetByIdAsync(request.IdProvincia, cancellationToken);
        if (provincia is null)
            throw new NotFoundException(nameof(Provincia), request.IdProvincia);

        var existente = await _unitOfWork.Provincias.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdProvincia != request.IdProvincia)
            throw new BusinessException($"Ya existe una provincia con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        provincia.Codigo = request.Codigo;
        provincia.Descripcion = request.Descripcion;
        provincia.CodigoAFIP = request.CodigoAFIP;
        provincia.Activo = request.Activo;
        provincia.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        provincia.IdUsuarioUltimaModificacion = userId;
        provincia.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Provincias.UpdateAsync(provincia, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<ProvinciaResponseDto>.Ok(_mapper.Map<ProvinciaResponseDto>(provincia), "Provincia actualizada correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var provincia = await _unitOfWork.Provincias.GetByIdAsync(id, cancellationToken);
        if (provincia is null)
            throw new NotFoundException(nameof(Provincia), id);

        if (!provincia.Activo)
            throw new BusinessException("La provincia ya se encuentra dada de baja.");

        var userId = _currentUserService.UserId ?? 1;

        provincia.Activo = false;
        provincia.FechaBaja = DateTime.UtcNow;
        provincia.IdUsuarioUltimaModificacion = userId;
        provincia.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Provincias.UpdateAsync(provincia, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Provincia dada de baja correctamente.");
    }
}
