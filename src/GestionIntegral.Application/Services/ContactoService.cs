using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Contactos;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class ContactoService : IContactoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public ContactoService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<ContactoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var contactos = await _unitOfWork.Contactos.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<ContactoResponseDto>>.Ok(_mapper.Map<IEnumerable<ContactoResponseDto>>(contactos));
    }

    public async Task<BaseResponseDto<ContactoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var contacto = await _unitOfWork.Contactos.GetByIdAsync(id, cancellationToken);
        if (contacto is null)
            throw new NotFoundException(nameof(Contacto), id);

        return BaseResponseDto<ContactoResponseDto>.Ok(_mapper.Map<ContactoResponseDto>(contacto));
    }

    public async Task<BaseResponseDto<ContactoResponseDto>> CrearAsync(CrearContactoRequestDto request, CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId ?? 1;

        var contacto = new Contacto
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Cargo = request.Cargo,
            Telefono = request.Telefono,
            Celular = request.Celular,
            Email = request.Email,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Contactos.AddAsync(contacto, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<ContactoResponseDto>.Ok(_mapper.Map<ContactoResponseDto>(contacto), "Contacto creado correctamente.");
    }

    public async Task<BaseResponseDto<ContactoResponseDto>> ActualizarAsync(ActualizarContactoRequestDto request, CancellationToken cancellationToken = default)
    {
        var contacto = await _unitOfWork.Contactos.GetByIdAsync(request.IdContacto, cancellationToken);
        if (contacto is null)
            throw new NotFoundException(nameof(Contacto), request.IdContacto);

        var userId = _currentUserService.UserId ?? 1;

        contacto.Nombre = request.Nombre;
        contacto.Apellido = request.Apellido;
        contacto.Cargo = request.Cargo;
        contacto.Telefono = request.Telefono;
        contacto.Celular = request.Celular;
        contacto.Email = request.Email;
        contacto.Activo = request.Activo;
        contacto.IdUsuarioUltimaModificacion = userId;
        contacto.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Contactos.UpdateAsync(contacto, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<ContactoResponseDto>.Ok(_mapper.Map<ContactoResponseDto>(contacto), "Contacto actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var contacto = await _unitOfWork.Contactos.GetByIdAsync(id, cancellationToken);
        if (contacto is null)
            throw new NotFoundException(nameof(Contacto), id);

        if (!contacto.Activo)
            throw new BusinessException("El contacto ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        contacto.Activo = false;
        contacto.IdUsuarioUltimaModificacion = userId;
        contacto.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Contactos.UpdateAsync(contacto, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Contacto dado de baja correctamente.");
    }
}
