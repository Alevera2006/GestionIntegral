using AutoMapper;
using FluentAssertions;
using GestionIntegral.Application.DTOs.Cajas;
using GestionIntegral.Application.Mappings;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class CajaDetalleServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly IMapper _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
    private readonly CajaDetalleService _service;

    public CajaDetalleServiceTests()
    {
        _service = new CajaDetalleService(_unitOfWorkMock.Object, _mapper, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_NoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.CajasDetalle.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((CajaDetalle?)null);

        Func<Task> act = async () => await _service.ObtenerPorIdAsync(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_ImporteNoValido_LanzaBusinessException()
    {
        var request = new CrearCajaDetalleRequestDto { IdCaja = 1, IdTipoMovimientoCaja = 2, Importe = 0 };
        _unitOfWorkMock.Setup(u => u.CajasCabecera.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new CajaCabecera { IdCaja = 1 });
        _unitOfWorkMock.Setup(u => u.TiposMovimientoCaja.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(new TipoMovimientoCaja { IdTipoMovimientoCaja = 2, Descripcion = "Ingreso" });

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task CrearAsync_DatosValidos_CreaDetalle()
    {
        var request = new CrearCajaDetalleRequestDto { IdCaja = 1, IdTipoMovimientoCaja = 2, Importe = 10, Concepto = "Ingreso" };
        _unitOfWorkMock.Setup(u => u.CajasCabecera.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new CajaCabecera { IdCaja = 1 });
        _unitOfWorkMock.Setup(u => u.TiposMovimientoCaja.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(new TipoMovimientoCaja { IdTipoMovimientoCaja = 2, Descripcion = "Ingreso" });
        _unitOfWorkMock.Setup(u => u.CajasDetalle.AddAsync(It.IsAny<CajaDetalle>(), It.IsAny<CancellationToken>())).ReturnsAsync((CajaDetalle detalle, CancellationToken _) => detalle);

        var result = await _service.CrearAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Importe.Should().Be(10);
    }

    [Fact]
    public async Task EliminarAsync_NoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.CajasDetalle.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((CajaDetalle?)null);

        Func<Task> act = async () => await _service.EliminarAsync(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
