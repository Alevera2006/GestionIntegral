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

public class CajaCabeceraServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly IMapper _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
    private readonly CajaCabeceraService _service;

    public CajaCabeceraServiceTests()
    {
        _service = new CajaCabeceraService(_unitOfWorkMock.Object, _mapper, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_NoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.CajasCabecera.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((CajaCabecera?)null);

        Func<Task> act = async () => await _service.ObtenerPorIdAsync(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_ResponsableNoExiste_LanzaNotFoundException()
    {
        var request = new CrearCajaCabeceraRequestDto { IdUsuarioResponsable = 1, IdEstadoCaja = 2, FechaCaja = DateTime.UtcNow, SaldoInicial = 10 };
        _unitOfWorkMock.Setup(u => u.Usuarios.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Usuario?)null);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_DatosValidos_CreaCaja()
    {
        var request = new CrearCajaCabeceraRequestDto { IdUsuarioResponsable = 1, IdEstadoCaja = 2, FechaCaja = DateTime.UtcNow, SaldoInicial = 10 };
        _unitOfWorkMock.Setup(u => u.Usuarios.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Usuario { IdUsuario = 1 });
        _unitOfWorkMock.Setup(u => u.EstadosCaja.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(new EstadoCaja { IdEstadoCaja = 2, Descripcion = "Abierta" });
        _unitOfWorkMock.Setup(u => u.CajasCabecera.AddAsync(It.IsAny<CajaCabecera>(), It.IsAny<CancellationToken>()))
            .Returns<CajaCabecera, CancellationToken>((c, _) => { c.IdCaja = 9; return Task.FromResult(c); });

        var result = await _service.CrearAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.IdCaja.Should().Be(9);
    }

    [Fact]
    public async Task EliminarAsync_NoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.CajasCabecera.GetByIdAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync((CajaCabecera?)null);

        Func<Task> act = async () => await _service.EliminarAsync(3);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
