using AutoMapper;
using FluentAssertions;
using GestionIntegral.Application.DTOs.Proveedores;
using GestionIntegral.Application.Mappings;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class ProveedorServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly IMapper _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
    private readonly ProveedorService _service;

    public ProveedorServiceTests()
    {
        _service = new ProveedorService(_unitOfWorkMock.Object, _mapper, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoNoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.Proveedores.GetByIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync((Proveedor?)null);

        Func<Task> act = async () => await _service.ObtenerPorIdAsync(10);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_CuandoExisteCodigo_LanzaBusinessException()
    {
        var request = new CrearProveedorRequestDto { Codigo = "PRV1", RazonSocial = "RS" };
        _unitOfWorkMock.Setup(u => u.Proveedores.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task CrearAsync_CuandoDatosValidos_CreaProveedor()
    {
        var request = new CrearProveedorRequestDto { Codigo = "PRV1", RazonSocial = "RS" };
        _unitOfWorkMock.Setup(u => u.Proveedores.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.Proveedores.AddAsync(It.IsAny<Proveedor>(), It.IsAny<CancellationToken>()))
            .Returns<Proveedor, CancellationToken>((p, _) => { p.IdProveedor = 20; return Task.FromResult(p); });

        var result = await _service.CrearAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.IdProveedor.Should().Be(20);
    }

    [Fact]
    public async Task ActualizarAsync_CuandoNoExiste_LanzaNotFoundException()
    {
        var request = new ActualizarProveedorRequestDto { IdProveedor = 99, Codigo = "PRV1", RazonSocial = "RS" };
        _unitOfWorkMock.Setup(u => u.Proveedores.GetByIdAsync(request.IdProveedor, It.IsAny<CancellationToken>())).ReturnsAsync((Proveedor?)null);

        Func<Task> act = async () => await _service.ActualizarAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task EliminarAsync_CuandoInactivo_LanzaBusinessException()
    {
        _unitOfWorkMock.Setup(u => u.Proveedores.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Proveedor { IdProveedor = 1, Activo = false });

        Func<Task> act = async () => await _service.EliminarAsync(1);

        await act.Should().ThrowAsync<BusinessException>();
    }
}
