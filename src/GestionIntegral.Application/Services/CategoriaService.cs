using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Categorias;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CategoriaService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<CategoriaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var categorias = await _unitOfWork.Categorias.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<CategoriaResponseDto>>.Ok(_mapper.Map<IEnumerable<CategoriaResponseDto>>(categorias));
    }

    public async Task<BaseResponseDto<CategoriaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var categoria = await _unitOfWork.Categorias.GetByIdAsync(id, cancellationToken);
        if (categoria is null)
            throw new NotFoundException(nameof(Categoria), id);

        return BaseResponseDto<CategoriaResponseDto>.Ok(_mapper.Map<CategoriaResponseDto>(categoria));
    }

    public async Task<BaseResponseDto<CategoriaResponseDto>> CrearAsync(CrearCategoriaRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Categorias.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe una categoría con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var categoria = new Categoria
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Categorias.AddAsync(categoria, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<CategoriaResponseDto>.Ok(_mapper.Map<CategoriaResponseDto>(categoria), "Categoría creada correctamente.");
    }

    public async Task<BaseResponseDto<CategoriaResponseDto>> ActualizarAsync(ActualizarCategoriaRequestDto request, CancellationToken cancellationToken = default)
    {
        var categoria = await _unitOfWork.Categorias.GetByIdAsync(request.IdCategoria, cancellationToken);
        if (categoria is null)
            throw new NotFoundException(nameof(Categoria), request.IdCategoria);

        var existente = await _unitOfWork.Categorias.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdCategoria != request.IdCategoria)
            throw new BusinessException($"Ya existe una categoría con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        categoria.Codigo = request.Codigo;
        categoria.Descripcion = request.Descripcion;
        categoria.Activo = request.Activo;
        categoria.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        categoria.IdUsuarioUltimaModificacion = userId;
        categoria.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Categorias.UpdateAsync(categoria, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<CategoriaResponseDto>.Ok(_mapper.Map<CategoriaResponseDto>(categoria), "Categoría actualizada correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var categoria = await _unitOfWork.Categorias.GetByIdAsync(id, cancellationToken);
        if (categoria is null)
            throw new NotFoundException(nameof(Categoria), id);

        if (!categoria.Activo)
            throw new BusinessException("La categoría ya se encuentra dada de baja.");

        var userId = _currentUserService.UserId ?? 1;

        categoria.Activo = false;
        categoria.FechaBaja = DateTime.UtcNow;
        categoria.IdUsuarioUltimaModificacion = userId;
        categoria.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Categorias.UpdateAsync(categoria, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Categoría dada de baja correctamente.");
    }
}
