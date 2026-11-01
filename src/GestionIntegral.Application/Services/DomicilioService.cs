using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Domicilios;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class DomicilioService : IDomicilioService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public DomicilioService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<DomicilioResponseDto>>> ObtenerTodosAsync(string? tipoReferencia, int? idReferencia, CancellationToken cancellationToken = default)
    {
        var tipo = string.IsNullOrWhiteSpace(tipoReferencia) ? null : tipoReferencia.Trim().ToUpperInvariant();
        var domicilios = (!string.IsNullOrWhiteSpace(tipo) || idReferencia.HasValue)
            ? await _unitOfWork.Domicilios.GetByOwnerAsync(tipo, idReferencia, cancellationToken)
            : await _unitOfWork.Domicilios.GetAllAsync(cancellationToken);

        return BaseResponseDto<IEnumerable<DomicilioResponseDto>>.Ok(domicilios.Select(MapToResponse));
    }

    public async Task<BaseResponseDto<DomicilioResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var domicilio = await _unitOfWork.Domicilios.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domicilio), id);

        return BaseResponseDto<DomicilioResponseDto>.Ok(MapToResponse(domicilio));
    }

    public async Task<BaseResponseDto<DomicilioResponseDto>> CrearAsync(CrearDomicilioRequestDto request, CancellationToken cancellationToken = default)
    {
        request.TipoReferencia = request.TipoReferencia.Trim().ToUpperInvariant();
        await ValidarOwnerAsync(request.TipoReferencia, request.IdReferencia, cancellationToken);

        var provincia = await _unitOfWork.Provincias.GetByIdAsync(request.IdProvincia, cancellationToken);
        if (provincia is null)
            throw new NotFoundException(nameof(Provincia), request.IdProvincia);

        var userId = _currentUserService.UserId ?? 1;
        var domicilio = new Domicilio
        {
            TipoReferencia = request.TipoReferencia,
            IdReferencia = request.IdReferencia,
            Direccion = request.Domicilio,
            Ciudad = request.Ciudad,
            Localidad = request.Localidad,
            IdProvincia = request.IdProvincia,
            CodigoPostal = request.CodigoPostal,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Domicilios.AddAsync(domicilio, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        domicilio = await _unitOfWork.Domicilios.GetByIdAsync(domicilio.IdDomicilio, cancellationToken) ?? domicilio;
        return BaseResponseDto<DomicilioResponseDto>.Ok(MapToResponse(domicilio), "Domicilio creado correctamente.");
    }

    public async Task<BaseResponseDto<DomicilioResponseDto>> ActualizarAsync(ActualizarDomicilioRequestDto request, CancellationToken cancellationToken = default)
    {
        var domicilio = await _unitOfWork.Domicilios.GetByIdAsync(request.IdDomicilio, cancellationToken)
            ?? throw new NotFoundException(nameof(Domicilio), request.IdDomicilio);

        request.TipoReferencia = request.TipoReferencia.Trim().ToUpperInvariant();
        await ValidarOwnerAsync(request.TipoReferencia, request.IdReferencia, cancellationToken);

        var provincia = await _unitOfWork.Provincias.GetByIdAsync(request.IdProvincia, cancellationToken);
        if (provincia is null)
            throw new NotFoundException(nameof(Provincia), request.IdProvincia);

        var userId = _currentUserService.UserId ?? 1;
        domicilio.TipoReferencia = request.TipoReferencia;
        domicilio.IdReferencia = request.IdReferencia;
        domicilio.Direccion = request.Domicilio;
        domicilio.Ciudad = request.Ciudad;
        domicilio.Localidad = request.Localidad;
        domicilio.IdProvincia = request.IdProvincia;
        domicilio.CodigoPostal = request.CodigoPostal;
        domicilio.Activo = request.Activo;
        domicilio.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        domicilio.IdUsuarioUltimaModificacion = userId;
        domicilio.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Domicilios.UpdateAsync(domicilio, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        domicilio = await _unitOfWork.Domicilios.GetByIdAsync(domicilio.IdDomicilio, cancellationToken) ?? domicilio;
        return BaseResponseDto<DomicilioResponseDto>.Ok(MapToResponse(domicilio), "Domicilio actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var domicilio = await _unitOfWork.Domicilios.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domicilio), id);

        if (!domicilio.Activo)
            throw new BusinessException("El domicilio ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;
        domicilio.Activo = false;
        domicilio.FechaBaja = DateTime.UtcNow;
        domicilio.IdUsuarioUltimaModificacion = userId;
        domicilio.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Domicilios.UpdateAsync(domicilio, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return BaseResponseDto<bool>.Ok(true, "Domicilio dado de baja correctamente.");
    }

    private async Task ValidarOwnerAsync(string tipoReferencia, int idReferencia, CancellationToken cancellationToken)
    {
        if (tipoReferencia is not ("C" or "P"))
            throw new BusinessException("El tipo de referencia debe ser C o P.");

        if (idReferencia <= 0)
            throw new BusinessException("El IdReferencia debe ser mayor que cero.");

        if (tipoReferencia == "C")
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(idReferencia, cancellationToken);
            if (cliente is null)
                throw new NotFoundException(nameof(Cliente), idReferencia);
        }
        else
        {
            var proveedor = await _unitOfWork.Proveedores.GetByIdAsync(idReferencia, cancellationToken);
            if (proveedor is null)
                throw new NotFoundException(nameof(Proveedor), idReferencia);
        }
    }

    private static DomicilioResponseDto MapToResponse(Domicilio entity) => new()
    {
        IdDomicilio = entity.IdDomicilio,
        TipoReferencia = entity.TipoReferencia,
        IdReferencia = entity.IdReferencia,
        Domicilio = entity.Direccion,
        Ciudad = entity.Ciudad,
        Localidad = entity.Localidad,
        IdProvincia = entity.IdProvincia,
        ProvinciaDescripcion = entity.Provincia?.Descripcion,
        CodigoPostal = entity.CodigoPostal,
        Activo = entity.Activo
    };
}
