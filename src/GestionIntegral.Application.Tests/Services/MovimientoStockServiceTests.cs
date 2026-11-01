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

public class MovimientoStockServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly IMapper _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
    private readonly MovimientoStockService _service;

    public MovimientoStockServiceTests()
    {
        _service = new MovimientoStockService(_unitOfWorkMock.Object, _mapper, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task CrearAsync_DepositoNoExiste_LanzaNotFoundException()
    {
        var request = new CrearMovimientoStockRequestDto { IdDeposito = 1, IdTipoMovimientoStock = 2, IdUsuarioResponsable = 3 };
        _unitOfWorkMock.Setup(u => u.Depositos.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Deposito?)null);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_DatosValidos_CreaMovimiento()
    {
        var request = new CrearMovimientoStockRequestDto { IdDeposito = 1, IdTipoMovimientoStock = 2, IdUsuarioResponsable = 3 };
        _unitOfWorkMock.Setup(u => u.Depositos.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Deposito { IdDeposito = 1, Codigo = "D1", Descripcion = "D" });
        _unitOfWorkMock.Setup(u => u.TiposMovimientoStock.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(new TipoMovimientoStock { IdTipoMovimientoStock = 2, Descripcion = "Ingreso" });
        _unitOfWorkMock.Setup(u => u.Usuarios.GetByIdAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(new Usuario { IdUsuario = 3 });
        _unitOfWorkMock.Setup(u => u.MovimientosStock.AddAsync(It.IsAny<MovimientoStock>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((MovimientoStock entity, CancellationToken _) => entity);

        var result = await _service.CrearAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.IdDeposito.Should().Be(1);
    }

    [Fact]
    public async Task ActualizarAsync_NoExiste_LanzaNotFoundException()
    {
        var request = new ActualizarMovimientoStockRequestDto { IdMovimientoStock = 1, IdDeposito = 1, IdTipoMovimientoStock = 2, IdUsuarioResponsable = 3 };
        _unitOfWorkMock.Setup(u => u.MovimientosStock.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((MovimientoStock?)null);

        Func<Task> act = async () => await _service.ActualizarAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task EliminarAsync_NoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.MovimientosStock.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((MovimientoStock?)null);

        Func<Task> act = async () => await _service.EliminarAsync(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
