using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Productos;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class ProductoService : IProductoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public ProductoService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<ProductoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var productos = await _unitOfWork.Productos.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<ProductoResponseDto>>.Ok(_mapper.Map<IEnumerable<ProductoResponseDto>>(productos));
    }

    public async Task<BaseResponseDto<ProductoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var producto = await _unitOfWork.Productos.GetByIdAsync(id, cancellationToken);
        if (producto is null)
            throw new NotFoundException(nameof(Producto), id);

        return BaseResponseDto<ProductoResponseDto>.Ok(_mapper.Map<ProductoResponseDto>(producto));
    }

    public async Task<BaseResponseDto<ProductoResponseDto>> CrearAsync(CrearProductoRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Productos.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe un producto con el código '{request.Codigo}'.");

        var categoria = await _unitOfWork.Categorias.GetByIdAsync(request.IdCategoria, cancellationToken);
        if (categoria is null)
            throw new NotFoundException(nameof(Categoria), request.IdCategoria);

        if (request.IdSubCategoria.HasValue)
        {
            var subCategoria = await _unitOfWork.SubCategorias.GetByIdAsync(request.IdSubCategoria.Value, cancellationToken);
            if (subCategoria is null || subCategoria.IdCategoria != request.IdCategoria)
                throw new BusinessException("La subcategoría no existe o no pertenece a la categoría seleccionada.");
        }

        var userId = _currentUserService.UserId ?? 1;

        var producto = new Producto
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            IdCategoria = request.IdCategoria,
            IdSubCategoria = request.IdSubCategoria,
            CodigoBarras = request.CodigoBarras,
            Costo = request.Costo,
            StockMinimo = request.StockMinimo,
            StockMaximo = request.StockMaximo,
            Observaciones = request.Observaciones,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Productos.AddAsync(producto, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<ProductoResponseDto>.Ok(_mapper.Map<ProductoResponseDto>(producto), "Producto creado correctamente.");
    }

    public async Task<BaseResponseDto<ProductoResponseDto>> ActualizarAsync(ActualizarProductoRequestDto request, CancellationToken cancellationToken = default)
    {
        var producto = await _unitOfWork.Productos.GetByIdAsync(request.IdProducto, cancellationToken);
        if (producto is null)
            throw new NotFoundException(nameof(Producto), request.IdProducto);

        var existente = await _unitOfWork.Productos.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdProducto != request.IdProducto)
            throw new BusinessException($"Ya existe un producto con el código '{request.Codigo}'.");

        var categoria = await _unitOfWork.Categorias.GetByIdAsync(request.IdCategoria, cancellationToken);
        if (categoria is null)
            throw new NotFoundException(nameof(Categoria), request.IdCategoria);

        if (request.IdSubCategoria.HasValue)
        {
            var subCategoria = await _unitOfWork.SubCategorias.GetByIdAsync(request.IdSubCategoria.Value, cancellationToken);
            if (subCategoria is null || subCategoria.IdCategoria != request.IdCategoria)
                throw new BusinessException("La subcategoría no existe o no pertenece a la categoría seleccionada.");
        }

        var userId = _currentUserService.UserId ?? 1;

        producto.Codigo = request.Codigo;
        producto.Descripcion = request.Descripcion;
        producto.IdCategoria = request.IdCategoria;
        producto.IdSubCategoria = request.IdSubCategoria;
        producto.CodigoBarras = request.CodigoBarras;
        producto.Costo = request.Costo;
        producto.StockMinimo = request.StockMinimo;
        producto.StockMaximo = request.StockMaximo;
        producto.Observaciones = request.Observaciones;
        producto.Activo = request.Activo;
        producto.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        producto.IdUsuarioUltimaModificacion = userId;
        producto.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Productos.UpdateAsync(producto, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<ProductoResponseDto>.Ok(_mapper.Map<ProductoResponseDto>(producto), "Producto actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var producto = await _unitOfWork.Productos.GetByIdAsync(id, cancellationToken);
        if (producto is null)
            throw new NotFoundException(nameof(Producto), id);

        if (!producto.Activo)
            throw new BusinessException("El producto ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        producto.Activo = false;
        producto.FechaBaja = DateTime.UtcNow;
        producto.IdUsuarioUltimaModificacion = userId;
        producto.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Productos.UpdateAsync(producto, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Producto dado de baja correctamente.");
    }
}
