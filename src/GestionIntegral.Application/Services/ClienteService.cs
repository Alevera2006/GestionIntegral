using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Clientes;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class ClienteService : IClienteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public ClienteService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<ClienteResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var clientes = await _unitOfWork.Clientes.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<ClienteResponseDto>>.Ok(_mapper.Map<IEnumerable<ClienteResponseDto>>(clientes));
    }

    public async Task<BaseResponseDto<ClienteResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(id, cancellationToken);
        if (cliente is null)
            throw new NotFoundException(nameof(Cliente), id);

        return BaseResponseDto<ClienteResponseDto>.Ok(_mapper.Map<ClienteResponseDto>(cliente));
    }

    public async Task<BaseResponseDto<ClienteResponseDto>> CrearAsync(CrearClienteRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Clientes.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe un cliente con el código '{request.Codigo}'.");

        if (request.IdCondicionPago.HasValue)
        {
            var condicion = await _unitOfWork.CondicionesPago.GetByIdAsync(request.IdCondicionPago.Value, cancellationToken);
            if (condicion is null)
                throw new NotFoundException(nameof(CondicionPago), request.IdCondicionPago.Value);
        }

        var userId = _currentUserService.UserId ?? 1;

        var cliente = new Cliente
        {
            Codigo = request.Codigo,
            RazonSocial = request.RazonSocial,
            NombreFantasia = request.NombreFantasia,
            CUIT = request.CUIT,
            Email = request.Email,
            Telefono = request.Telefono,
            IdCondicionPago = request.IdCondicionPago,
            Observaciones = request.Observaciones,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Clientes.AddAsync(cliente, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<ClienteResponseDto>.Ok(_mapper.Map<ClienteResponseDto>(cliente), "Cliente creado correctamente.");
    }

    public async Task<BaseResponseDto<ClienteResponseDto>> ActualizarAsync(ActualizarClienteRequestDto request, CancellationToken cancellationToken = default)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(request.IdCliente, cancellationToken);
        if (cliente is null)
            throw new NotFoundException(nameof(Cliente), request.IdCliente);

        var existente = await _unitOfWork.Clientes.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdCliente != request.IdCliente)
            throw new BusinessException($"Ya existe un cliente con el código '{request.Codigo}'.");

        if (request.IdCondicionPago.HasValue)
        {
            var condicion = await _unitOfWork.CondicionesPago.GetByIdAsync(request.IdCondicionPago.Value, cancellationToken);
            if (condicion is null)
                throw new NotFoundException(nameof(CondicionPago), request.IdCondicionPago.Value);
        }

        var userId = _currentUserService.UserId ?? 1;

        cliente.Codigo = request.Codigo;
        cliente.RazonSocial = request.RazonSocial;
        cliente.NombreFantasia = request.NombreFantasia;
        cliente.CUIT = request.CUIT;
        cliente.Email = request.Email;
        cliente.Telefono = request.Telefono;
        cliente.IdCondicionPago = request.IdCondicionPago;
        cliente.Observaciones = request.Observaciones;
        cliente.Activo = request.Activo;
        cliente.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        cliente.IdUsuarioUltimaModificacion = userId;
        cliente.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Clientes.UpdateAsync(cliente, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<ClienteResponseDto>.Ok(_mapper.Map<ClienteResponseDto>(cliente), "Cliente actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(id, cancellationToken);
        if (cliente is null)
            throw new NotFoundException(nameof(Cliente), id);

        if (!cliente.Activo)
            throw new BusinessException("El cliente ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        cliente.Activo = false;
        cliente.FechaBaja = DateTime.UtcNow;
        cliente.IdUsuarioUltimaModificacion = userId;
        cliente.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.Clientes.UpdateAsync(cliente, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Cliente dado de baja correctamente.");
    }
}
