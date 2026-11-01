using AutoMapper;
using FluentAssertions;
using GestionIntegral.Application.DTOs.Stock;
using GestionIntegral.Application.Mappings;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class StockServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly IMapper _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
    private readonly StockService _service;

    public StockServiceTests()
    {
        _service = new StockService(_unitOfWorkMock.Object, _mapper, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task CrearAsync_DepositoNoExiste_LanzaNotFoundException()
    {
        var request = new CrearStockRequestDto { IdDeposito = 1, IdProducto = 2, Cantidad = 10 };
        _unitOfWorkMock.Setup(u => u.Depositos.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Deposito?)null);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_RegistroExistente_LanzaBusinessException()
    {
        var request = new CrearStockRequestDto { IdDeposito = 1, IdProducto = 2, Cantidad = 10 };
        _unitOfWorkMock.Setup(u => u.Depositos.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Deposito { IdDeposito = 1, Codigo = "D1", Descripcion = "D" });
        _unitOfWorkMock.Setup(u => u.Productos.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(new Producto { IdProducto = 2 });
        _unitOfWorkMock.Setup(u => u.Stock.GetByDepositoProductoAsync(1, 2, It.IsAny<CancellationToken>())).ReturnsAsync(new Stock());

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task CrearAsync_DatosValidos_CreaStock()
    {
        var request = new CrearStockRequestDto { IdDeposito = 1, IdProducto = 2, Cantidad = 10 };
        _unitOfWorkMock.Setup(u => u.Depositos.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Deposito { IdDeposito = 1, Codigo = "D1", Descripcion = "D" });
        _unitOfWorkMock.Setup(u => u.Productos.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(new Producto { IdProducto = 2 });
        _unitOfWorkMock.Setup(u => u.Stock.GetByDepositoProductoAsync(1, 2, It.IsAny<CancellationToken>())).ReturnsAsync((Stock?)null);

        var result = await _service.CrearAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Cantidad.Should().Be(10);
    }

    [Fact]
    public async Task EliminarAsync_NoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.Stock.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Stock?)null);

        Func<Task> act = async () => await _service.EliminarAsync(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
