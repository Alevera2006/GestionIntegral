using AutoMapper;
using FluentAssertions;
using GestionIntegral.Application.DTOs.MovimientosStock;
using GestionIntegral.Application.Mappings;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class MovimientoStockDetalleServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly IMapper _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
    private readonly MovimientoStockDetalleService _service;

    public MovimientoStockDetalleServiceTests()
    {
        _service = new MovimientoStockDetalleService(_unitOfWorkMock.Object, _mapper, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task CrearAsync_MovimientoNoExiste_LanzaNotFoundException()
    {
        var request = new CrearMovimientoStockDetalleRequestDto { IdMovimientoStock = 1, IdProducto = 2, Cantidad = 1 };
        _unitOfWorkMock.Setup(u => u.MovimientosStock.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((MovimientoStock?)null);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_CantidadNoValida_LanzaBusinessException()
    {
        var request = new CrearMovimientoStockDetalleRequestDto { IdMovimientoStock = 1, IdProducto = 2, Cantidad = 0 };
        _unitOfWorkMock.Setup(u => u.MovimientosStock.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new MovimientoStock { IdMovimientoStock = 1, IdTipoMovimientoStock = 1 });
        _unitOfWorkMock.Setup(u => u.TiposMovimientoStock.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new TipoMovimientoStock { IdTipoMovimientoStock = 1, Signo = 1 });
        _unitOfWorkMock.Setup(u => u.Productos.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(new Producto { IdProducto = 2 });

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task CrearAsync_DatosValidos_CreaDetalle()
    {
        var request = new CrearMovimientoStockDetalleRequestDto { IdMovimientoStock = 1, IdProducto = 2, Cantidad = 3 };
        _unitOfWorkMock.Setup(u => u.MovimientosStock.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new MovimientoStock { IdMovimientoStock = 1, IdTipoMovimientoStock = 1, IdDeposito = 1 });
        _unitOfWorkMock.Setup(u => u.TiposMovimientoStock.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new TipoMovimientoStock { IdTipoMovimientoStock = 1, Signo = 1 });
        _unitOfWorkMock.Setup(u => u.Productos.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(new Producto { IdProducto = 2 });
        _unitOfWorkMock.Setup(u => u.MovimientoStockDetalle.AddAsync(It.IsAny<MovimientoStockDetalle>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((MovimientoStockDetalle entity, CancellationToken _) => entity);
        _unitOfWorkMock.Setup(u => u.Stock.GetByDepositoProductoTrackedAsync(1, 2, It.IsAny<CancellationToken>())).ReturnsAsync((Stock?)null);
        _unitOfWorkMock.Setup(u => u.Stock.AddAsync(It.IsAny<Stock>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Stock entity, CancellationToken _) => entity);

        var result = await _service.CrearAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Cantidad.Should().Be(3);
    }

    [Fact]
    public async Task EliminarAsync_NoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.MovimientoStockDetalle.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((MovimientoStockDetalle?)null);

        Func<Task> act = async () => await _service.EliminarAsync(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
