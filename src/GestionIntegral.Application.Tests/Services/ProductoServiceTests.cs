using AutoMapper;
using FluentAssertions;
using GestionIntegral.Application.DTOs.Productos;
using GestionIntegral.Application.Mappings;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class ProductoServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly IMapper _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
    private readonly ProductoService _service;

    public ProductoServiceTests()
    {
        _service = new ProductoService(_unitOfWorkMock.Object, _mapper, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task CrearAsync_CategoriaNoExiste_LanzaNotFoundException()
    {
        var request = new CrearProductoRequestDto { Codigo = "P1", Descripcion = "Prod", IdCategoria = 3 };
        _unitOfWorkMock.Setup(u => u.Productos.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.Categorias.GetByIdAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync((Categoria?)null);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_SubcategoriaInvalida_LanzaBusinessException()
    {
        var request = new CrearProductoRequestDto { Codigo = "P1", Descripcion = "Prod", IdCategoria = 3, IdSubCategoria = 8 };
        _unitOfWorkMock.Setup(u => u.Productos.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.Categorias.GetByIdAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(new Categoria { IdCategoria = 3 });
        _unitOfWorkMock.Setup(u => u.SubCategorias.GetByIdAsync(8, It.IsAny<CancellationToken>())).ReturnsAsync(new SubCategoria { IdSubCategoria = 8, IdCategoria = 9 });

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task CrearAsync_DatosValidos_CreaProducto()
    {
        var request = new CrearProductoRequestDto { Codigo = "P1", Descripcion = "Prod", IdCategoria = 3 };
        _unitOfWorkMock.Setup(u => u.Productos.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.Categorias.GetByIdAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(new Categoria { IdCategoria = 3 });

        var result = await _service.CrearAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Codigo.Should().Be("P1");
    }

    [Fact]
    public async Task EliminarAsync_Inactivo_LanzaBusinessException()
    {
        _unitOfWorkMock.Setup(u => u.Productos.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Producto { IdProducto = 1, Activo = false });

        Func<Task> act = async () => await _service.EliminarAsync(1);

        await act.Should().ThrowAsync<BusinessException>();
    }
}
