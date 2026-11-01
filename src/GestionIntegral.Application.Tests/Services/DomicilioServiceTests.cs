using FluentAssertions;
using GestionIntegral.Application.DTOs.Domicilios;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class DomicilioServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly DomicilioService _service;

    public DomicilioServiceTests()
    {
        _service = new DomicilioService(_unitOfWorkMock.Object, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_NoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.Domicilios.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Domicilio?)null);

        Func<Task> act = async () => await _service.ObtenerPorIdAsync(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_TipoReferenciaInvalido_LanzaBusinessException()
    {
        var request = new CrearDomicilioRequestDto { TipoReferencia = "X", IdReferencia = 1, Domicilio = "Calle 1", Ciudad = "Ciudad", IdProvincia = 1 };

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task CrearAsync_ProvinciaNoExiste_LanzaNotFoundException()
    {
        var request = new CrearDomicilioRequestDto { TipoReferencia = "C", IdReferencia = 1, Domicilio = "Calle 1", Ciudad = "Ciudad", IdProvincia = 9 };
        _unitOfWorkMock.Setup(u => u.Clientes.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Cliente { IdCliente = 1 });
        _unitOfWorkMock.Setup(u => u.Provincias.GetByIdAsync(9, It.IsAny<CancellationToken>())).ReturnsAsync((Provincia?)null);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task EliminarAsync_Inactivo_LanzaBusinessException()
    {
        _unitOfWorkMock.Setup(u => u.Domicilios.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Domicilio { IdDomicilio = 1, Activo = false, TipoReferencia = "C", IdReferencia = 1, Direccion = "Dir", Ciudad = "C", IdProvincia = 1 });

        Func<Task> act = async () => await _service.EliminarAsync(1);

        await act.Should().ThrowAsync<BusinessException>();
    }
}
