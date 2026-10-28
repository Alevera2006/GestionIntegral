using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Usuarios;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;
    private readonly ICurrentUserService _currentUserService;

    public UsuarioService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordService passwordService, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordService = passwordService;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<UsuarioResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var usuarios = await _unitOfWork.Usuarios.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<UsuarioResponseDto>>.Ok(_mapper.Map<IEnumerable<UsuarioResponseDto>>(usuarios));
    }

    public async Task<BaseResponseDto<UsuarioResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id, cancellationToken);
        if (usuario is null)
            throw new NotFoundException(nameof(Usuario), id);

        return BaseResponseDto<UsuarioResponseDto>.Ok(_mapper.Map<UsuarioResponseDto>(usuario));
    }

    public async Task<BaseResponseDto<UsuarioResponseDto>> CrearAsync(CrearUsuarioRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Usuarios.ExisteUsuarioAsync(request.UsuarioNombre, cancellationToken))
            throw new BusinessException($"Ya existe un usuario con el nombre '{request.UsuarioNombre}'.");

        var tipoUsuario = await _unitOfWork.TiposUsuario.GetByIdAsync(request.IdTipoUsuario, cancellationToken);
        if (tipoUsuario is null)
            throw new NotFoundException(nameof(TipoUsuario), request.IdTipoUsuario);

        var userId = _currentUserService.UserId ?? 1;

        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            IdTipoUsuario = request.IdTipoUsuario,
            UsuarioNombre = request.UsuarioNombre,
            ClaveHash = _passwordService.Hash(request.Clave),
            Bloqueado = false,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Usuarios.AddAsync(usuario, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<UsuarioResponseDto>.Ok(_mapper.Map<UsuarioResponseDto>(usuario), "Usuario creado correctamente.");
    }

    public async Task<BaseResponseDto<UsuarioResponseDto>> ActualizarAsync(ActualizarUsuarioRequestDto request, CancellationToken cancellationToken = default)
    {
        var usuario = await _unitOfWork.Usuarios.GetByIdAsync(request.IdUsuario, cancellationToken);
        if (usuario is null)
            throw new NotFoundException(nameof(Usuario), request.IdUsuario);

        var tipoUsuario = await _unitOfWork.TiposUsuario.GetByIdAsync(request.IdTipoUsuario, cancellationToken);
        if (tipoUsuario is null)
            throw new NotFoundException(nameof(TipoUsuario), request.IdTipoUsuario);

        var userId = _currentUserService.UserId ?? 1;

        usuario.Nombre = request.Nombre;
        usuario.Apellido = request.Apellido;
        usuario.IdTipoUsuario = request.IdTipoUsuario;
        usuario.Bloqueado = request.Bloqueado;
        usuario.Activo = request.Activo;
        usuario.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        usuario.IdUsuarioUltimaModificacion = userId;
        usuario.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Usuarios.UpdateAsync(usuario, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<UsuarioResponseDto>.Ok(_mapper.Map<UsuarioResponseDto>(usuario), "Usuario actualizado correctamente.");
    }
}
