using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.ClienteContactos;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;

namespace GestionIntegral.Application.Services;

public class ClienteContactoService : IClienteContactoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ClienteContactoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponseDto<IEnumerable<ClienteContactoResponseDto>>> ObtenerPorClienteAsync(int idCliente, CancellationToken cancellationToken = default)
    {
        var relaciones = await _unitOfWork.ClienteContactos.GetByClienteAsync(idCliente, cancellationToken);
        return BaseResponseDto<IEnumerable<ClienteContactoResponseDto>>.Ok(_mapper.Map<IEnumerable<ClienteContactoResponseDto>>(relaciones));
    }

    public async Task<BaseResponseDto<ClienteContactoResponseDto>> CrearAsync(CrearClienteContactoRequestDto request, CancellationToken cancellationToken = default)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(request.IdCliente, cancellationToken);
        if (cliente is null)
            throw new NotFoundException(nameof(Cliente), request.IdCliente);

        var contacto = await _unitOfWork.Contactos.GetByIdAsync(request.IdContacto, cancellationToken);
        if (contacto is null)
            throw new NotFoundException(nameof(Contacto), request.IdContacto);

        var existente = await _unitOfWork.ClienteContactos.GetByIdsAsync(request.IdCliente, request.IdContacto, cancellationToken);
        if (existente is not null)
            throw new BusinessException("El contacto ya se encuentra asociado al cliente.");

        var relacion = new ClienteContacto
        {
            IdCliente = request.IdCliente,
            IdContacto = request.IdContacto,
            EsPrincipal = request.EsPrincipal
        };

        await _unitOfWork.ClienteContactos.AddAsync(relacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<ClienteContactoResponseDto>.Ok(_mapper.Map<ClienteContactoResponseDto>(relacion), "Contacto asociado al cliente correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int idCliente, int idContacto, CancellationToken cancellationToken = default)
    {
        var relacion = await _unitOfWork.ClienteContactos.GetByIdsAsync(idCliente, idContacto, cancellationToken);
        if (relacion is null)
            throw new NotFoundException(nameof(ClienteContacto), $"{idCliente}-{idContacto}");

        await _unitOfWork.ClienteContactos.DeleteAsync(relacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Contacto desasociado del cliente correctamente.");
    }
}
