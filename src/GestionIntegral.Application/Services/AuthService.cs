using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Auth;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IPasswordService _passwordService;

    public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService, IPasswordService passwordService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _passwordService = passwordService;
    }

    public async Task<BaseResponseDto<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var usuario = await _unitOfWork.Usuarios.GetByUsuarioNombreAsync(request.Usuario, cancellationToken);

        if (usuario is null || !_passwordService.Verify(request.Clave, usuario.ClaveHash))
            return BaseResponseDto<LoginResponseDto>.Error("Usuario o clave incorrectos.");

        if (usuario.Bloqueado)
            return BaseResponseDto<LoginResponseDto>.Error("El usuario se encuentra bloqueado.");

        if (!usuario.Activo)
            return BaseResponseDto<LoginResponseDto>.Error("El usuario se encuentra inactivo.");

        var token = _tokenService.GenerarToken(usuario);

        var response = new LoginResponseDto
        {
            Token = token,
            Usuario = usuario.UsuarioNombre,
            NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}",
            IdTipoUsuario = usuario.IdTipoUsuario
        };

        return BaseResponseDto<LoginResponseDto>.Ok(response);
    }
}
