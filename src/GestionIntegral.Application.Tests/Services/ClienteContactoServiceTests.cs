using AutoMapper;
using FluentAssertions;
using GestionIntegral.Application.DTOs.ClienteContactos;
using GestionIntegral.Application.Mappings;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class ClienteContactoServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly IMapper _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
    private readonly ClienteContactoService _service;

    public ClienteContactoServiceTests()
    {
        _service = new ClienteContactoService(_unitOfWorkMock.Object, _mapper);
    }

    [Fact]
    public async Task CrearAsync_ClienteNoExiste_LanzaNotFoundException()
    {
        var request = new CrearClienteContactoRequestDto { IdCliente = 1, IdContacto = 2 };
        _unitOfWorkMock.Setup(u => u.Clientes.GetByIdAsync(request.IdCliente, It.IsAny<CancellationToken>())).ReturnsAsync((Cliente?)null);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_YaExisteRelacion_LanzaBusinessException()
    {
        var request = new CrearClienteContactoRequestDto { IdCliente = 1, IdContacto = 2 };
        _unitOfWorkMock.Setup(u => u.Clientes.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Cliente { IdCliente = 1 });
        _unitOfWorkMock.Setup(u => u.Contactos.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(new Contacto { IdContacto = 2, Nombre = "A" });
        _unitOfWorkMock.Setup(u => u.ClienteContactos.GetByIdsAsync(1, 2, It.IsAny<CancellationToken>())).ReturnsAsync(new ClienteContacto());

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task CrearAsync_DatosValidos_CreaRelacion()
    {
        var request = new CrearClienteContactoRequestDto { IdCliente = 1, IdContacto = 2, EsPrincipal = true };
        _unitOfWorkMock.Setup(u => u.Clientes.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Cliente { IdCliente = 1 });
        _unitOfWorkMock.Setup(u => u.Contactos.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(new Contacto { IdContacto = 2, Nombre = "A" });
        _unitOfWorkMock.Setup(u => u.ClienteContactos.GetByIdsAsync(1, 2, It.IsAny<CancellationToken>())).ReturnsAsync((ClienteContacto?)null);

        var result = await _service.CrearAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.IdCliente.Should().Be(1);
    }

    [Fact]
    public async Task EliminarAsync_NoExisteRelacion_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.ClienteContactos.GetByIdsAsync(1, 2, It.IsAny<CancellationToken>())).ReturnsAsync((ClienteContacto?)null);

        Func<Task> act = async () => await _service.EliminarAsync(1, 2);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
