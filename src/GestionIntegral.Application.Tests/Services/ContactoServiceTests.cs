using AutoMapper;
using FluentAssertions;
using GestionIntegral.Application.DTOs.Contactos;
using GestionIntegral.Application.Mappings;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class ContactoServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly IMapper _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
    private readonly ContactoService _service;

    public ContactoServiceTests()
    {
        _service = new ContactoService(_unitOfWorkMock.Object, _mapper, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task ObtenerTodosAsync_RetornaLista()
    {
        _unitOfWorkMock.Setup(u => u.Contactos.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Contacto> { new() { IdContacto = 1, Nombre = "N" } });

        var result = await _service.ObtenerTodosAsync();

        result.Exitoso.Should().BeTrue();
        result.Datos.Should().HaveCount(1);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_NoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.Contactos.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Contacto?)null);

        Func<Task> act = async () => await _service.ObtenerPorIdAsync(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ActualizarAsync_NoExiste_LanzaNotFoundException()
    {
        var request = new ActualizarContactoRequestDto { IdContacto = 1, Nombre = "N", Activo = true };
        _unitOfWorkMock.Setup(u => u.Contactos.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Contacto?)null);

        Func<Task> act = async () => await _service.ActualizarAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task EliminarAsync_Inactivo_LanzaBusinessException()
    {
        _unitOfWorkMock.Setup(u => u.Contactos.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Contacto { IdContacto = 1, Activo = false, Nombre = "N" });

        Func<Task> act = async () => await _service.EliminarAsync(1);

        await act.Should().ThrowAsync<BusinessException>();
    }
}
