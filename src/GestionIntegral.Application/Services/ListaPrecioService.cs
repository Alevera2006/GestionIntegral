using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.ListaPrecioDetalles;
using GestionIntegral.Application.DTOs.ListasPrecios;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class ListaPrecioService : IListaPrecioService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public ListaPrecioService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<ListaPrecioResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var listas = await _unitOfWork.ListasPrecios.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<ListaPrecioResponseDto>>.Ok(_mapper.Map<IEnumerable<ListaPrecioResponseDto>>(listas));
    }

    public async Task<BaseResponseDto<ListaPrecioResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var lista = await _unitOfWork.ListasPrecios.GetByIdAsync(id, cancellationToken);
        if (lista is null)
            throw new NotFoundException(nameof(ListaPrecio), id);

        return BaseResponseDto<ListaPrecioResponseDto>.Ok(_mapper.Map<ListaPrecioResponseDto>(lista));
    }

    public async Task<BaseResponseDto<ListaPrecioResponseDto>> CrearAsync(CrearListaPrecioRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.ListasPrecios.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe una lista de precios con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var lista = new ListaPrecio
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            FechaDesde = request.FechaDesde,
            FechaHasta = request.FechaHasta,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.ListasPrecios.AddAsync(lista, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<ListaPrecioResponseDto>.Ok(_mapper.Map<ListaPrecioResponseDto>(lista), "Lista de precios creada correctamente.");
    }

    public async Task<BaseResponseDto<ListaPrecioResponseDto>> ActualizarAsync(ActualizarListaPrecioRequestDto request, CancellationToken cancellationToken = default)
    {
        var lista = await _unitOfWork.ListasPrecios.GetByIdAsync(request.IdListaPrecio, cancellationToken);
        if (lista is null)
            throw new NotFoundException(nameof(ListaPrecio), request.IdListaPrecio);

        var existente = await _unitOfWork.ListasPrecios.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdListaPrecio != request.IdListaPrecio)
            throw new BusinessException($"Ya existe una lista de precios con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        lista.Codigo = request.Codigo;
        lista.Descripcion = request.Descripcion;
        lista.FechaDesde = request.FechaDesde;
        lista.FechaHasta = request.FechaHasta;
        lista.Activo = request.Activo;
        lista.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        lista.IdUsuarioUltimaModificacion = userId;
        lista.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.ListasPrecios.UpdateAsync(lista, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<ListaPrecioResponseDto>.Ok(_mapper.Map<ListaPrecioResponseDto>(lista), "Lista de precios actualizada correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var lista = await _unitOfWork.ListasPrecios.GetByIdAsync(id, cancellationToken);
        if (lista is null)
            throw new NotFoundException(nameof(ListaPrecio), id);

        if (!lista.Activo)
            throw new BusinessException("La lista de precios ya se encuentra dada de baja.");

        var userId = _currentUserService.UserId ?? 1;

        lista.Activo = false;
        lista.FechaBaja = DateTime.UtcNow;
        lista.IdUsuarioUltimaModificacion = userId;
        lista.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.ListasPrecios.UpdateAsync(lista, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Lista de precios dada de baja correctamente.");
    }

    public async Task<BaseResponseDto<IEnumerable<ListaPrecioDetalleResponseDto>>> ObtenerDetallesAsync(int idListaPrecio, CancellationToken cancellationToken = default)
    {
        var lista = await _unitOfWork.ListasPrecios.GetByIdAsync(idListaPrecio, cancellationToken);
        if (lista is null)
            throw new NotFoundException(nameof(ListaPrecio), idListaPrecio);

        var detalles = await _unitOfWork.ListaPrecioDetalles.GetByListaPrecioAsync(idListaPrecio, cancellationToken);
        return BaseResponseDto<IEnumerable<ListaPrecioDetalleResponseDto>>.Ok(_mapper.Map<IEnumerable<ListaPrecioDetalleResponseDto>>(detalles));
    }

    public async Task<BaseResponseDto<ListaPrecioDetalleResponseDto>> AgregarDetalleAsync(CrearListaPrecioDetalleRequestDto request, CancellationToken cancellationToken = default)
    {
        var lista = await _unitOfWork.ListasPrecios.GetByIdAsync(request.IdListaPrecio, cancellationToken);
        if (lista is null)
            throw new NotFoundException(nameof(ListaPrecio), request.IdListaPrecio);

        var producto = await _unitOfWork.Productos.GetByIdAsync(request.IdProducto, cancellationToken);
        if (producto is null)
            throw new NotFoundException(nameof(Producto), request.IdProducto);

        if (await _unitOfWork.ListaPrecioDetalles.ExisteProductoEnListaAsync(request.IdListaPrecio, request.IdProducto, cancellationToken))
            throw new BusinessException("El producto ya se encuentra en la lista de precios.");

        var userId = _currentUserService.UserId ?? 1;

        var detalle = new ListaPrecioDetalle
        {
            IdListaPrecio = request.IdListaPrecio,
            IdProducto = request.IdProducto,
            Precio = request.Precio,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.ListaPrecioDetalles.AddAsync(detalle, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<ListaPrecioDetalleResponseDto>.Ok(_mapper.Map<ListaPrecioDetalleResponseDto>(detalle), "Producto agregado a la lista de precios correctamente.");
    }

    public async Task<BaseResponseDto<ListaPrecioDetalleResponseDto>> ActualizarDetalleAsync(ActualizarListaPrecioDetalleRequestDto request, CancellationToken cancellationToken = default)
    {
        var detalle = await _unitOfWork.ListaPrecioDetalles.GetByIdAsync(request.IdListaPrecioDetalle, cancellationToken);
        if (detalle is null)
            throw new NotFoundException(nameof(ListaPrecioDetalle), request.IdListaPrecioDetalle);

        var userId = _currentUserService.UserId ?? 1;

        detalle.Precio = request.Precio;
        detalle.IdUsuarioUltimaModificacion = userId;
        detalle.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.ListaPrecioDetalles.UpdateAsync(detalle, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<ListaPrecioDetalleResponseDto>.Ok(_mapper.Map<ListaPrecioDetalleResponseDto>(detalle), "Detalle actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarDetalleAsync(int idListaPrecioDetalle, CancellationToken cancellationToken = default)
    {
        var detalle = await _unitOfWork.ListaPrecioDetalles.GetByIdAsync(idListaPrecioDetalle, cancellationToken);
        if (detalle is null)
            throw new NotFoundException(nameof(ListaPrecioDetalle), idListaPrecioDetalle);

        await _unitOfWork.ListaPrecioDetalles.DeleteAsync(detalle, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Detalle eliminado correctamente.");
    }
}
