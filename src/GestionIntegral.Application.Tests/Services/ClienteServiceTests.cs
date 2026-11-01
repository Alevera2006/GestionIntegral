using AutoMapper;
using FluentAssertions;
using GestionIntegral.Application.DTOs.Clientes;
using GestionIntegral.Application.Mappings;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class ClienteServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly IMapper _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
    private readonly ClienteService _service;

    public ClienteServiceTests()
    {
        _service = new ClienteService(_unitOfWorkMock.Object, _mapper, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoNoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.Clientes.GetByIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync((Cliente?)null);

        Func<Task> act = async () => await _service.ObtenerPorIdAsync(10);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_CuandoExisteCodigo_LanzaBusinessException()
    {
        var request = new CrearClienteRequestDto { Codigo = "CLI1", RazonSocial = "RS" };
        _unitOfWorkMock.Setup(u => u.Clientes.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task CrearAsync_CuandoCondicionPagoNoExiste_LanzaNotFoundException()
    {
        var request = new CrearClienteRequestDto { Codigo = "CLI1", RazonSocial = "RS", IdCondicionPago = 2 };
        _unitOfWorkMock.Setup(u => u.Clientes.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.CondicionesPago.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync((CondicionPago?)null);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_CuandoDatosValidos_CreaCliente()
    {
        var request = new CrearClienteRequestDto { Codigo = "CLI1", RazonSocial = "RS" };
        _unitOfWorkMock.Setup(u => u.Clientes.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.Clientes.AddAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()))
            .Returns<Cliente, CancellationToken>((c, _) => { c.IdCliente = 15; return Task.FromResult(c); });
        _currentUserServiceMock.Setup(c => c.UserId).Returns(7);

        var result = await _service.CrearAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.IdCliente.Should().Be(15);
    }

    [Fact]
    public async Task EliminarAsync_CuandoInactivo_LanzaBusinessException()
    {
        _unitOfWorkMock.Setup(u => u.Clientes.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Cliente { IdCliente = 1, Activo = false });

        Func<Task> act = async () => await _service.EliminarAsync(1);

        await act.Should().ThrowAsync<BusinessException>();
    }
}
