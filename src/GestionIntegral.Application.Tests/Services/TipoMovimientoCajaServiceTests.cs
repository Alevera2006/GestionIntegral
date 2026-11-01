using AutoMapper;
using FluentAssertions;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.TiposMovimientoCaja;
using GestionIntegral.Application.Mappings;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class TipoMovimientoCajaServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly TipoMovimientoCajaService _service;

    public TipoMovimientoCajaServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _service = new TipoMovimientoCajaService(_unitOfWorkMock.Object, _mapper, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task ObtenerTodosAsync_RetornaListaMapeada()
    {
        var entidades = new List<TipoMovimientoCaja> { new() { IdTipoMovimientoCaja = 1, Codigo = "COD1", Descripcion = "Desc 1" } };
        _unitOfWorkMock.Setup(u => u.TiposMovimientoCaja.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(entidades);

        var result = await _service.ObtenerTodosAsync();

        result.Exitoso.Should().BeTrue();
        result.Datos.Should().HaveCount(1);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoExiste_RetornaDto()
    {
        var entidad = new TipoMovimientoCaja { IdTipoMovimientoCaja = 1, Codigo = "COD1", Descripcion = "Desc 1" };
        _unitOfWorkMock.Setup(u => u.TiposMovimientoCaja.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entidad);

        var result = await _service.ObtenerPorIdAsync(1);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Codigo.Should().Be("COD1");
    }

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoNoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.TiposMovimientoCaja.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((TipoMovimientoCaja?)null);

        Func<Task> act = async () => await _service.ObtenerPorIdAsync(99);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_CuandoNoExiste_CreaCorrectamente()
    {
        var request = new CrearTipoMovimientoCajaRequestDto { Codigo = "COD1", Descripcion = "Desc 1" };
            request.Signo = 1;
        _unitOfWorkMock.Setup(u => u.TiposMovimientoCaja.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.TiposMovimientoCaja.AddAsync(It.IsAny<TipoMovimientoCaja>(), It.IsAny<CancellationToken>()))
            .Returns<TipoMovimientoCaja, CancellationToken>((e, _) => { e.IdTipoMovimientoCaja = 10; return Task.FromResult(e); });
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _currentUserServiceMock.Setup(c => c.UserId).Returns(5);

        var result = await _service.CrearAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Codigo.Should().Be("COD1");
        result.Datos.IdTipoMovimientoCaja.Should().Be(10);
    }

    [Fact]
    public async Task CrearAsync_CuandoExisteCodigo_LanzaBusinessException()
    {
        var request = new CrearTipoMovimientoCajaRequestDto { Codigo = "COD1", Descripcion = "Desc 1" };
        _unitOfWorkMock.Setup(u => u.TiposMovimientoCaja.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task ActualizarAsync_CuandoExiste_ActualizaCorrectamente()
    {
        var entidad = new TipoMovimientoCaja { IdTipoMovimientoCaja = 1, Codigo = "COD1", Descripcion = "Desc 1", Signo = 1, Activo = true };
        var request = new ActualizarTipoMovimientoCajaRequestDto { IdTipoMovimientoCaja = 1, Codigo = "COD2", Descripcion = "Desc 2", Signo = -1, Activo = true };
        _unitOfWorkMock.Setup(u => u.TiposMovimientoCaja.GetByIdAsync(request.IdTipoMovimientoCaja, It.IsAny<CancellationToken>())).ReturnsAsync(entidad);
        _unitOfWorkMock.Setup(u => u.TiposMovimientoCaja.GetByCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync((TipoMovimientoCaja?)null);
        _unitOfWorkMock.Setup(u => u.TiposMovimientoCaja.UpdateAsync(It.IsAny<TipoMovimientoCaja>(), It.IsAny<CancellationToken>()))
            .Returns<TipoMovimientoCaja, CancellationToken>((e, _) => Task.FromResult(e));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _currentUserServiceMock.Setup(c => c.UserId).Returns(5);

        var result = await _service.ActualizarAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Codigo.Should().Be("COD2");
    }

    [Fact]
    public async Task ActualizarAsync_CuandoNoExiste_LanzaNotFoundException()
    {
        var request = new ActualizarTipoMovimientoCajaRequestDto { IdTipoMovimientoCaja = 99, Codigo = "COD1", Descripcion = "Desc 1", Activo = true };
        _unitOfWorkMock.Setup(u => u.TiposMovimientoCaja.GetByIdAsync(request.IdTipoMovimientoCaja, It.IsAny<CancellationToken>())).ReturnsAsync((TipoMovimientoCaja?)null);

        Func<Task> act = async () => await _service.ActualizarAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task EliminarAsync_CuandoExisteYActivo_DaDeBajaCorrectamente()
    {
        var entidad = new TipoMovimientoCaja { IdTipoMovimientoCaja = 1, Codigo = "COD1", Descripcion = "Desc 1", Activo = true };
        _unitOfWorkMock.Setup(u => u.TiposMovimientoCaja.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entidad);
        _unitOfWorkMock.Setup(u => u.TiposMovimientoCaja.UpdateAsync(It.IsAny<TipoMovimientoCaja>(), It.IsAny<CancellationToken>()))
            .Returns<TipoMovimientoCaja, CancellationToken>((e, _) => Task.FromResult(e));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _currentUserServiceMock.Setup(c => c.UserId).Returns(5);

        var result = await _service.EliminarAsync(1);

        result.Exitoso.Should().BeTrue();
        result.Datos.Should().BeTrue();
        entidad.Activo.Should().BeFalse();
    }

    [Fact]
    public async Task EliminarAsync_CuandoNoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.TiposMovimientoCaja.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((TipoMovimientoCaja?)null);

        Func<Task> act = async () => await _service.EliminarAsync(99);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
