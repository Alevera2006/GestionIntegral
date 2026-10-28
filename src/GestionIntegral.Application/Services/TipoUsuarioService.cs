using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.TiposUsuario;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class TipoUsuarioService : ITipoUsuarioService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public TipoUsuarioService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<TipoUsuarioResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var tipos = await _unitOfWork.TiposUsuario.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<TipoUsuarioResponseDto>>.Ok(_mapper.Map<IEnumerable<TipoUsuarioResponseDto>>(tipos));
    }

    public async Task<BaseResponseDto<TipoUsuarioResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var tipo = await _unitOfWork.TiposUsuario.GetByIdAsync(id, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoUsuario), id);

        return BaseResponseDto<TipoUsuarioResponseDto>.Ok(_mapper.Map<TipoUsuarioResponseDto>(tipo));
    }

    public async Task<BaseResponseDto<TipoUsuarioResponseDto>> CrearAsync(CrearTipoUsuarioRequestDto request, CancellationToken cancellationToken = default)
    {
        var existente = await _unitOfWork.TiposUsuario.GetByDescripcionAsync(request.Descripcion, cancellationToken);
        if (existente is not null)
            throw new BusinessException($"Ya existe un tipo de usuario con la descripción '{request.Descripcion}'.");

        var userId = _currentUserService.UserId ?? 1;

        var tipo = new TipoUsuario
        {
            Descripcion = request.Descripcion,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.TiposUsuario.AddAsync(tipo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<TipoUsuarioResponseDto>.Ok(_mapper.Map<TipoUsuarioResponseDto>(tipo), "Tipo de usuario creado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var tipo = await _unitOfWork.TiposUsuario.GetByIdAsync(id, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoUsuario), id);

        if (!tipo.Activo)
            throw new BusinessException("El tipo de usuario ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        tipo.Activo = false;
        tipo.FechaBaja = DateTime.UtcNow;
        tipo.IdUsuarioUltimaModificacion = userId;
        tipo.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.TiposUsuario.UpdateAsync(tipo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Tipo de usuario dado de baja correctamente.");
    }
}
