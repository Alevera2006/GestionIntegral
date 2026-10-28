using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.SubCategorias;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class SubCategoriaService : ISubCategoriaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public SubCategoriaService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<SubCategoriaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var subCategorias = await _unitOfWork.SubCategorias.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<SubCategoriaResponseDto>>.Ok(_mapper.Map<IEnumerable<SubCategoriaResponseDto>>(subCategorias));
    }

    public async Task<BaseResponseDto<IEnumerable<SubCategoriaResponseDto>>> ObtenerPorCategoriaAsync(int idCategoria, CancellationToken cancellationToken = default)
    {
        var subCategorias = await _unitOfWork.SubCategorias.GetByCategoriaAsync(idCategoria, cancellationToken);
        return BaseResponseDto<IEnumerable<SubCategoriaResponseDto>>.Ok(_mapper.Map<IEnumerable<SubCategoriaResponseDto>>(subCategorias));
    }

    public async Task<BaseResponseDto<SubCategoriaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var subCategoria = await _unitOfWork.SubCategorias.GetByIdAsync(id, cancellationToken);
        if (subCategoria is null)
            throw new NotFoundException(nameof(SubCategoria), id);

        return BaseResponseDto<SubCategoriaResponseDto>.Ok(_mapper.Map<SubCategoriaResponseDto>(subCategoria));
    }

    public async Task<BaseResponseDto<SubCategoriaResponseDto>> CrearAsync(CrearSubCategoriaRequestDto request, CancellationToken cancellationToken = default)
    {
        var categoria = await _unitOfWork.Categorias.GetByIdAsync(request.IdCategoria, cancellationToken);
        if (categoria is null)
            throw new NotFoundException(nameof(Categoria), request.IdCategoria);

        if (await _unitOfWork.SubCategorias.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe una subcategoría con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var subCategoria = new SubCategoria
        {
            IdCategoria = request.IdCategoria,
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.SubCategorias.AddAsync(subCategoria, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<SubCategoriaResponseDto>.Ok(_mapper.Map<SubCategoriaResponseDto>(subCategoria), "Subcategoría creada correctamente.");
    }

    public async Task<BaseResponseDto<SubCategoriaResponseDto>> ActualizarAsync(ActualizarSubCategoriaRequestDto request, CancellationToken cancellationToken = default)
    {
        var subCategoria = await _unitOfWork.SubCategorias.GetByIdAsync(request.IdSubCategoria, cancellationToken);
        if (subCategoria is null)
            throw new NotFoundException(nameof(SubCategoria), request.IdSubCategoria);

        var categoria = await _unitOfWork.Categorias.GetByIdAsync(request.IdCategoria, cancellationToken);
        if (categoria is null)
            throw new NotFoundException(nameof(Categoria), request.IdCategoria);

        var existente = await _unitOfWork.SubCategorias.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdSubCategoria != request.IdSubCategoria)
            throw new BusinessException($"Ya existe una subcategoría con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        subCategoria.IdCategoria = request.IdCategoria;
        subCategoria.Codigo = request.Codigo;
        subCategoria.Descripcion = request.Descripcion;
        subCategoria.Activo = request.Activo;
        subCategoria.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        subCategoria.IdUsuarioUltimaModificacion = userId;
        subCategoria.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.SubCategorias.UpdateAsync(subCategoria, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<SubCategoriaResponseDto>.Ok(_mapper.Map<SubCategoriaResponseDto>(subCategoria), "Subcategoría actualizada correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var subCategoria = await _unitOfWork.SubCategorias.GetByIdAsync(id, cancellationToken);
        if (subCategoria is null)
            throw new NotFoundException(nameof(SubCategoria), id);

        if (!subCategoria.Activo)
            throw new BusinessException("La subcategoría ya se encuentra dada de baja.");

        var userId = _currentUserService.UserId ?? 1;

        subCategoria.Activo = false;
        subCategoria.FechaBaja = DateTime.UtcNow;
        subCategoria.IdUsuarioUltimaModificacion = userId;
        subCategoria.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.SubCategorias.UpdateAsync(subCategoria, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Subcategoría dada de baja correctamente.");
    }
}
