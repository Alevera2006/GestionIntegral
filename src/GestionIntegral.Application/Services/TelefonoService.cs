using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Telefonos;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class TelefonoService : ITelefonoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public TelefonoService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<TelefonoResponseDto>>> ObtenerTodosAsync(string? tipoReferencia, int? idReferencia, CancellationToken cancellationToken = default)
    {
        var tipo = string.IsNullOrWhiteSpace(tipoReferencia) ? null : tipoReferencia.Trim().ToUpperInvariant();

        var telefonos = (!string.IsNullOrWhiteSpace(tipo) || idReferencia.HasValue)
            ? await _unitOfWork.Telefonos.GetByOwnerAsync(tipo, idReferencia, cancellationToken)
            : await _unitOfWork.Telefonos.GetAllAsync(cancellationToken);

        return BaseResponseDto<IEnumerable<TelefonoResponseDto>>.Ok(telefonos.Select(MapToResponse));
    }

    public async Task<BaseResponseDto<TelefonoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var telefono = await _unitOfWork.Telefonos.GetByIdAsync(id, cancellationToken);
        if (telefono is null)
            throw new NotFoundException(nameof(Telefono), id);

        return BaseResponseDto<TelefonoResponseDto>.Ok(MapToResponse(telefono));
    }

    public async Task<BaseResponseDto<TelefonoResponseDto>> CrearAsync(CrearTelefonoRequestDto request, CancellationToken cancellationToken = default)
    {
        request.TipoReferencia = request.TipoReferencia.Trim().ToUpperInvariant();
        await ValidarOwnerAsync(request.TipoReferencia, request.IdReferencia, cancellationToken);

        var userId = _currentUserService.UserId ?? 1;

        var telefono = new Telefono
        {
            TipoReferencia = request.TipoReferencia,
            IdReferencia = request.IdReferencia,
            NumeroTelefono = request.NumeroTelefono,
            TipoTelefono = request.TipoTelefono,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Telefonos.AddAsync(telefono, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<TelefonoResponseDto>.Ok(MapToResponse(telefono), "Teléfono creado correctamente.");
    }

    public async Task<BaseResponseDto<TelefonoResponseDto>> ActualizarAsync(ActualizarTelefonoRequestDto request, CancellationToken cancellationToken = default)
    {
        var telefono = await _unitOfWork.Telefonos.GetByIdAsync(request.IdTelefono, cancellationToken);
        if (telefono is null)
            throw new NotFoundException(nameof(Telefono), request.IdTelefono);

        request.TipoReferencia = request.TipoReferencia.Trim().ToUpperInvariant();
        await ValidarOwnerAsync(request.TipoReferencia, request.IdReferencia, cancellationToken);

        var userId = _currentUserService.UserId ?? 1;

        telefono.TipoReferencia = request.TipoReferencia;
        telefono.IdReferencia = request.IdReferencia;
        telefono.NumeroTelefono = request.NumeroTelefono;
        telefono.TipoTelefono = request.TipoTelefono;
        telefono.Activo = request.Activo;
        telefono.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        telefono.IdUsuarioUltimaModificacion = userId;
        telefono.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Telefonos.UpdateAsync(telefono, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<TelefonoResponseDto>.Ok(MapToResponse(telefono), "Teléfono actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var telefono = await _unitOfWork.Telefonos.GetByIdAsync(id, cancellationToken);
        if (telefono is null)
            throw new NotFoundException(nameof(Telefono), id);

        if (!telefono.Activo)
            throw new BusinessException("El teléfono ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        telefono.Activo = false;
        telefono.FechaBaja = DateTime.UtcNow;
        telefono.IdUsuarioUltimaModificacion = userId;
        telefono.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Telefonos.UpdateAsync(telefono, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Teléfono dado de baja correctamente.");
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

    private static TelefonoResponseDto MapToResponse(Telefono entity) => new()
    {
        IdTelefono = entity.IdTelefono,
        TipoReferencia = entity.TipoReferencia,
        IdReferencia = entity.IdReferencia,
        NumeroTelefono = entity.NumeroTelefono,
        TipoTelefono = entity.TipoTelefono,
        Activo = entity.Activo
    };
}
