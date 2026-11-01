using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Emails;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class EmailService : IEmailService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public EmailService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<EmailResponseDto>>> ObtenerTodosAsync(string? tipoReferencia, int? idReferencia, CancellationToken cancellationToken = default)
    {
        var tipo = string.IsNullOrWhiteSpace(tipoReferencia) ? null : tipoReferencia.Trim().ToUpperInvariant();

        var emails = (!string.IsNullOrWhiteSpace(tipo) || idReferencia.HasValue)
            ? await _unitOfWork.Emails.GetByOwnerAsync(tipo, idReferencia, cancellationToken)
            : await _unitOfWork.Emails.GetAllAsync(cancellationToken);

        return BaseResponseDto<IEnumerable<EmailResponseDto>>.Ok(emails.Select(MapToResponse));
    }

    public async Task<BaseResponseDto<EmailResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var email = await _unitOfWork.Emails.GetByIdAsync(id, cancellationToken);
        if (email is null)
            throw new NotFoundException(nameof(Email), id);

        return BaseResponseDto<EmailResponseDto>.Ok(MapToResponse(email));
    }

    public async Task<BaseResponseDto<EmailResponseDto>> CrearAsync(CrearEmailRequestDto request, CancellationToken cancellationToken = default)
    {
        await ValidarOwnerAsync(request.TipoReferencia, request.IdReferencia, cancellationToken);

        var userId = _currentUserService.UserId ?? 1;

        request.TipoReferencia = request.TipoReferencia.Trim().ToUpperInvariant();

        var email = new Domain.Entities.Email
        {
            TipoReferencia = request.TipoReferencia,
            IdReferencia = request.IdReferencia,
            DireccionEmail = request.Email,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Emails.AddAsync(email, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<EmailResponseDto>.Ok(MapToResponse(email), "Email creado correctamente.");
    }

    public async Task<BaseResponseDto<EmailResponseDto>> ActualizarAsync(ActualizarEmailRequestDto request, CancellationToken cancellationToken = default)
    {
        var email = await _unitOfWork.Emails.GetByIdAsync(request.IdEmail, cancellationToken);
        if (email is null)
            throw new NotFoundException(nameof(Email), request.IdEmail);

        await ValidarOwnerAsync(request.TipoReferencia, request.IdReferencia, cancellationToken);

        var userId = _currentUserService.UserId ?? 1;

        request.TipoReferencia = request.TipoReferencia.Trim().ToUpperInvariant();
        email.TipoReferencia = request.TipoReferencia;
        email.IdReferencia = request.IdReferencia;
        email.DireccionEmail = request.Email;
        email.Activo = request.Activo;
        email.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        email.IdUsuarioUltimaModificacion = userId;
        email.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Emails.UpdateAsync(email, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<EmailResponseDto>.Ok(MapToResponse(email), "Email actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var email = await _unitOfWork.Emails.GetByIdAsync(id, cancellationToken);
        if (email is null)
            throw new NotFoundException(nameof(Email), id);

        if (!email.Activo)
            throw new BusinessException("El email ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        email.Activo = false;
        email.FechaBaja = DateTime.UtcNow;
        email.IdUsuarioUltimaModificacion = userId;
        email.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Emails.UpdateAsync(email, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Email dado de baja correctamente.");
    }

    private async Task ValidarOwnerAsync(string tipoReferencia, int idReferencia, CancellationToken cancellationToken)
    {
        tipoReferencia = tipoReferencia?.Trim().ToUpperInvariant() ?? string.Empty;

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

    private static EmailResponseDto MapToResponse(Domain.Entities.Email entity) => new()
    {
        IdEmail = entity.IdEmail,
        TipoReferencia = entity.TipoReferencia,
        IdReferencia = entity.IdReferencia,
        Email = entity.DireccionEmail,
        Activo = entity.Activo
    };
}
