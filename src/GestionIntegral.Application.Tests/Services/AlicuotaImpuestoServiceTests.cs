using AutoMapper;
using FluentAssertions;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.AlicuotasImpuesto;
using GestionIntegral.Application.Mappings;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class AlicuotaImpuestoServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly AlicuotaImpuestoService _service;

    public AlicuotaImpuestoServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _service = new AlicuotaImpuestoService(_unitOfWorkMock.Object, _mapper, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task ObtenerTodosAsync_RetornaListaMapeada()
    {
        var entidades = new List<AlicuotaImpuesto> { new() { IdAlicuotaImpuesto = 1, Codigo = "COD1", Descripcion = "Desc 1" } };
        _unitOfWorkMock.Setup(u => u.AlicuotasImpuesto.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(entidades);

        var result = await _service.ObtenerTodosAsync();

        result.Exitoso.Should().BeTrue();
        result.Datos.Should().HaveCount(1);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoExiste_RetornaDto()
    {
        var entidad = new AlicuotaImpuesto { IdAlicuotaImpuesto = 1, Codigo = "COD1", Descripcion = "Desc 1" };
        _unitOfWorkMock.Setup(u => u.AlicuotasImpuesto.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entidad);

        var result = await _service.ObtenerPorIdAsync(1);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Codigo.Should().Be("COD1");
    }

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoNoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.AlicuotasImpuesto.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((AlicuotaImpuesto?)null);

        Func<Task> act = async () => await _service.ObtenerPorIdAsync(99);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_CuandoNoExiste_CreaCorrectamente()
    {
        var request = new CrearAlicuotaImpuestoRequestDto { Codigo = "COD1", Descripcion = "Desc 1" };
            request.IdTipoImpuesto = 1;
            request.Porcentaje = 21;
            request.FechaDesde = DateTime.UtcNow;
            request.FechaHasta = DateTime.UtcNow;
        var tipoImpuesto = new TipoImpuesto { IdTipoImpuesto = request.IdTipoImpuesto, Codigo = "IVA", Descripcion = "IVA" };
        _unitOfWorkMock.Setup(u => u.TiposImpuesto.GetByIdAsync(request.IdTipoImpuesto, It.IsAny<CancellationToken>())).ReturnsAsync(tipoImpuesto);
        _unitOfWorkMock.Setup(u => u.AlicuotasImpuesto.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.AlicuotasImpuesto.AddAsync(It.IsAny<AlicuotaImpuesto>(), It.IsAny<CancellationToken>()))
            .Returns<AlicuotaImpuesto, CancellationToken>((e, _) => { e.IdAlicuotaImpuesto = 10; return Task.FromResult(e); });
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _currentUserServiceMock.Setup(c => c.UserId).Returns(5);

        var result = await _service.CrearAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Codigo.Should().Be("COD1");
        result.Datos.IdAlicuotaImpuesto.Should().Be(10);
    }

    [Fact]
    public async Task CrearAsync_CuandoExisteCodigo_LanzaBusinessException()
    {
        var request = new CrearAlicuotaImpuestoRequestDto { Codigo = "COD1", Descripcion = "Desc 1" };
        _unitOfWorkMock.Setup(u => u.AlicuotasImpuesto.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task ActualizarAsync_CuandoExiste_ActualizaCorrectamente()
    {
        var entidad = new AlicuotaImpuesto { IdAlicuotaImpuesto = 1, IdTipoImpuesto = 1, Codigo = "COD1", Descripcion = "Desc 1", Activo = true };
        var request = new ActualizarAlicuotaImpuestoRequestDto { IdAlicuotaImpuesto = 1, IdTipoImpuesto = 1, Codigo = "COD2", Descripcion = "Desc 2", Activo = true };
        var tipoImpuesto = new TipoImpuesto { IdTipoImpuesto = request.IdTipoImpuesto, Codigo = "IVA", Descripcion = "IVA" };
        _unitOfWorkMock.Setup(u => u.AlicuotasImpuesto.GetByIdAsync(request.IdAlicuotaImpuesto, It.IsAny<CancellationToken>())).ReturnsAsync(entidad);
        _unitOfWorkMock.Setup(u => u.TiposImpuesto.GetByIdAsync(request.IdTipoImpuesto, It.IsAny<CancellationToken>())).ReturnsAsync(tipoImpuesto);
        _unitOfWorkMock.Setup(u => u.AlicuotasImpuesto.GetByCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync((AlicuotaImpuesto?)null);
        _unitOfWorkMock.Setup(u => u.AlicuotasImpuesto.UpdateAsync(It.IsAny<AlicuotaImpuesto>(), It.IsAny<CancellationToken>()))
            .Returns<AlicuotaImpuesto, CancellationToken>((e, _) => Task.FromResult(e));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _currentUserServiceMock.Setup(c => c.UserId).Returns(5);

        var result = await _service.ActualizarAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Codigo.Should().Be("COD2");
    }

    [Fact]
    public async Task ActualizarAsync_CuandoNoExiste_LanzaNotFoundException()
    {
        var request = new ActualizarAlicuotaImpuestoRequestDto { IdTipoImpuesto = 99, Codigo = "COD1", Descripcion = "Desc 1", Activo = true };
        _unitOfWorkMock.Setup(u => u.AlicuotasImpuesto.GetByIdAsync(request.IdTipoImpuesto, It.IsAny<CancellationToken>())).ReturnsAsync((AlicuotaImpuesto?)null);

        Func<Task> act = async () => await _service.ActualizarAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task EliminarAsync_CuandoExisteYActivo_DaDeBajaCorrectamente()
    {
        var entidad = new AlicuotaImpuesto { IdAlicuotaImpuesto = 1, Codigo = "COD1", Descripcion = "Desc 1", Activo = true };
        _unitOfWorkMock.Setup(u => u.AlicuotasImpuesto.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entidad);
        _unitOfWorkMock.Setup(u => u.AlicuotasImpuesto.UpdateAsync(It.IsAny<AlicuotaImpuesto>(), It.IsAny<CancellationToken>()))
            .Returns<AlicuotaImpuesto, CancellationToken>((e, _) => Task.FromResult(e));
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
        _unitOfWorkMock.Setup(u => u.AlicuotasImpuesto.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((AlicuotaImpuesto?)null);

        Func<Task> act = async () => await _service.EliminarAsync(99);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
