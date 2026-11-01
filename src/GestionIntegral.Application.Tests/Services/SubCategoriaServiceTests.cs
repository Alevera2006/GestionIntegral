using AutoMapper;
using FluentAssertions;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.SubCategorias;
using GestionIntegral.Application.Mappings;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class SubCategoriaServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly SubCategoriaService _service;

    public SubCategoriaServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _service = new SubCategoriaService(_unitOfWorkMock.Object, _mapper, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task ObtenerTodosAsync_RetornaListaMapeada()
    {
        var entidades = new List<SubCategoria> { new() { IdSubCategoria = 1, Codigo = "COD1", Descripcion = "Desc 1" } };
        _unitOfWorkMock.Setup(u => u.SubCategorias.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(entidades);

        var result = await _service.ObtenerTodosAsync();

        result.Exitoso.Should().BeTrue();
        result.Datos.Should().HaveCount(1);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoExiste_RetornaDto()
    {
        var entidad = new SubCategoria { IdSubCategoria = 1, Codigo = "COD1", Descripcion = "Desc 1" };
        _unitOfWorkMock.Setup(u => u.SubCategorias.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entidad);

        var result = await _service.ObtenerPorIdAsync(1);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Codigo.Should().Be("COD1");
    }

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoNoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.SubCategorias.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((SubCategoria?)null);

        Func<Task> act = async () => await _service.ObtenerPorIdAsync(99);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_CuandoNoExiste_CreaCorrectamente()
    {
        var request = new CrearSubCategoriaRequestDto { Codigo = "COD1", Descripcion = "Desc 1" };
            request.IdCategoria = 1;
        var categoria = new Categoria { IdCategoria = request.IdCategoria, Codigo = "CAT1", Descripcion = "Categoria 1" };
        _unitOfWorkMock.Setup(u => u.Categorias.GetByIdAsync(request.IdCategoria, It.IsAny<CancellationToken>())).ReturnsAsync(categoria);
        _unitOfWorkMock.Setup(u => u.SubCategorias.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.SubCategorias.AddAsync(It.IsAny<SubCategoria>(), It.IsAny<CancellationToken>()))
            .Returns<SubCategoria, CancellationToken>((e, _) => { e.IdSubCategoria = 10; return Task.FromResult(e); });
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _currentUserServiceMock.Setup(c => c.UserId).Returns(5);

        var result = await _service.CrearAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Codigo.Should().Be("COD1");
        result.Datos.IdSubCategoria.Should().Be(10);
    }

    [Fact]
    public async Task CrearAsync_CuandoExisteCodigo_LanzaBusinessException()
    {
        var request = new CrearSubCategoriaRequestDto { Codigo = "COD1", Descripcion = "Desc 1" };
        request.IdCategoria = 1;
        var categoria = new Categoria { IdCategoria = request.IdCategoria, Codigo = "CAT1", Descripcion = "Categoria 1" };
        _unitOfWorkMock.Setup(u => u.Categorias.GetByIdAsync(request.IdCategoria, It.IsAny<CancellationToken>())).ReturnsAsync(categoria);
        _unitOfWorkMock.Setup(u => u.SubCategorias.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task ActualizarAsync_CuandoExiste_ActualizaCorrectamente()
    {
        var entidad = new SubCategoria { IdSubCategoria = 1, IdCategoria = 1, Codigo = "COD1", Descripcion = "Desc 1", Activo = true };
        var request = new ActualizarSubCategoriaRequestDto { IdSubCategoria = 1, IdCategoria = 1, Codigo = "COD2", Descripcion = "Desc 2", Activo = true };
        var categoria = new Categoria { IdCategoria = request.IdCategoria, Codigo = "CAT1", Descripcion = "Categoria 1" };
        _unitOfWorkMock.Setup(u => u.SubCategorias.GetByIdAsync(request.IdSubCategoria, It.IsAny<CancellationToken>())).ReturnsAsync(entidad);
        _unitOfWorkMock.Setup(u => u.Categorias.GetByIdAsync(request.IdCategoria, It.IsAny<CancellationToken>())).ReturnsAsync(categoria);
        _unitOfWorkMock.Setup(u => u.SubCategorias.GetByCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync((SubCategoria?)null);
        _unitOfWorkMock.Setup(u => u.SubCategorias.UpdateAsync(It.IsAny<SubCategoria>(), It.IsAny<CancellationToken>()))
            .Returns<SubCategoria, CancellationToken>((e, _) => Task.FromResult(e));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _currentUserServiceMock.Setup(c => c.UserId).Returns(5);

        var result = await _service.ActualizarAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Codigo.Should().Be("COD2");
    }

    [Fact]
    public async Task ActualizarAsync_CuandoNoExiste_LanzaNotFoundException()
    {
        var request = new ActualizarSubCategoriaRequestDto { IdCategoria = 99, Codigo = "COD1", Descripcion = "Desc 1", Activo = true };
        _unitOfWorkMock.Setup(u => u.SubCategorias.GetByIdAsync(request.IdCategoria, It.IsAny<CancellationToken>())).ReturnsAsync((SubCategoria?)null);

        Func<Task> act = async () => await _service.ActualizarAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task EliminarAsync_CuandoExisteYActivo_DaDeBajaCorrectamente()
    {
        var entidad = new SubCategoria { IdSubCategoria = 1, Codigo = "COD1", Descripcion = "Desc 1", Activo = true };
        _unitOfWorkMock.Setup(u => u.SubCategorias.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entidad);
        _unitOfWorkMock.Setup(u => u.SubCategorias.UpdateAsync(It.IsAny<SubCategoria>(), It.IsAny<CancellationToken>()))
            .Returns<SubCategoria, CancellationToken>((e, _) => Task.FromResult(e));
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
        _unitOfWorkMock.Setup(u => u.SubCategorias.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((SubCategoria?)null);

        Func<Task> act = async () => await _service.EliminarAsync(99);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
