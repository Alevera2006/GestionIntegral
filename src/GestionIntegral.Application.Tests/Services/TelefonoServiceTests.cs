using FluentAssertions;
using GestionIntegral.Application.DTOs.Telefonos;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class TelefonoServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly TelefonoService _service;

    public TelefonoServiceTests()
    {
        _service = new TelefonoService(_unitOfWorkMock.Object, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_NoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.Telefonos.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Telefono?)null);

        Func<Task> act = async () => await _service.ObtenerPorIdAsync(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_TipoReferenciaInvalido_LanzaBusinessException()
    {
        var request = new CrearTelefonoRequestDto { TipoReferencia = "X", IdReferencia = 1, NumeroTelefono = "1", TipoTelefono = "M" };

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task CrearAsync_ProveedorNoExiste_LanzaNotFoundException()
    {
        var request = new CrearTelefonoRequestDto { TipoReferencia = "P", IdReferencia = 2, NumeroTelefono = "1", TipoTelefono = "M" };
        _unitOfWorkMock.Setup(u => u.Proveedores.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync((Proveedor?)null);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task EliminarAsync_Inactivo_LanzaBusinessException()
    {
        _unitOfWorkMock.Setup(u => u.Telefonos.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Telefono { IdTelefono = 1, Activo = false, NumeroTelefono = "1", TipoTelefono = "M", TipoReferencia = "C", IdReferencia = 1 });

        Func<Task> act = async () => await _service.EliminarAsync(1);

        await act.Should().ThrowAsync<BusinessException>();
    }
}
