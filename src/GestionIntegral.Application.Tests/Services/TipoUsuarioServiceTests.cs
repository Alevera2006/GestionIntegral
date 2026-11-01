using AutoMapper;
using FluentAssertions;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.TiposUsuario;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class TipoUsuarioServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly TipoUsuarioService _service;

    public TipoUsuarioServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _service = new TipoUsuarioService(_unitOfWorkMock.Object, _mapperMock.Object, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task ObtenerTodosAsync_RetornaListaMapeada()
    {
        // Arrange
        var tipos = new List<TipoUsuario>
        {
            new() { IdTipoUsuario = 1, Descripcion = "Administrador" },
            new() { IdTipoUsuario = 2, Descripcion = "Operador" }
        };

        var dtos = new List<TipoUsuarioResponseDto>
        {
            new() { IdTipoUsuario = 1, Descripcion = "Administrador" },
            new() { IdTipoUsuario = 2, Descripcion = "Operador" }
        };

        _unitOfWorkMock.Setup(u => u.TiposUsuario.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tipos);
        _mapperMock.Setup(m => m.Map<IEnumerable<TipoUsuarioResponseDto>>(tipos)).Returns(dtos);

        // Act
        var result = await _service.ObtenerTodosAsync();

        // Assert
        result.Exitoso.Should().BeTrue();
        result.Datos.Should().BeEquivalentTo(dtos);
    }

    [Fact]
    public async Task CrearAsync_CuandoNoExiste_CreaCorrectamente()
    {
        // Arrange
        var request = new CrearTipoUsuarioRequestDto { Descripcion = "Nuevo Tipo" };

        _unitOfWorkMock.Setup(u => u.TiposUsuario.GetByDescripcionAsync(request.Descripcion, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TipoUsuario?)null);

        _unitOfWorkMock.Setup(u => u.TiposUsuario.AddAsync(It.IsAny<TipoUsuario>(), It.IsAny<CancellationToken>()))
            .Returns<TipoUsuario, CancellationToken>((t, _) =>
            {
                t.IdTipoUsuario = 10;
                return Task.FromResult(t);
            });

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _mapperMock.Setup(m => m.Map<TipoUsuarioResponseDto>(It.IsAny<TipoUsuario>()))
            .Returns<TipoUsuario>(t => new TipoUsuarioResponseDto
            {
                IdTipoUsuario = t.IdTipoUsuario,
                Descripcion = t.Descripcion
            });

        // Act
        var result = await _service.CrearAsync(request);

        // Assert
        result.Exitoso.Should().BeTrue();
        result.Datos.Should().NotBeNull();
        result.Datos!.Descripcion.Should().Be(request.Descripcion);
        result.Datos.IdTipoUsuario.Should().Be(10);
        _unitOfWorkMock.Verify(u => u.TiposUsuario.AddAsync(It.Is<TipoUsuario>(t => t.Descripcion == request.Descripcion), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CrearAsync_CuandoExiste_LanzaBusinessException()
    {
        // Arrange
        var request = new CrearTipoUsuarioRequestDto { Descripcion = "Administrador" };
        var existente = new TipoUsuario { IdTipoUsuario = 1, Descripcion = request.Descripcion };

        _unitOfWorkMock.Setup(u => u.TiposUsuario.GetByDescripcionAsync(request.Descripcion, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existente);

        // Act
        Func<Task> act = async () => await _service.CrearAsync(request);

        // Assert
        await act.Should().ThrowAsync<BusinessException>()
            .WithMessage($"Ya existe un tipo de usuario con la descripción '{request.Descripcion}'.");
    }

    [Fact]
    public async Task EliminarAsync_CuandoExisteYEstaActivo_DaDeBajaCorrectamente()
    {
        // Arrange
        var tipo = new TipoUsuario { IdTipoUsuario = 1, Descripcion = "Administrador", Activo = true };

        _unitOfWorkMock.Setup(u => u.TiposUsuario.GetByIdAsync(tipo.IdTipoUsuario, It.IsAny<CancellationToken>())).ReturnsAsync(tipo);
        _unitOfWorkMock.Setup(u => u.TiposUsuario.UpdateAsync(tipo, It.IsAny<CancellationToken>())).ReturnsAsync(tipo);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _currentUserServiceMock.Setup(c => c.UserId).Returns(5);

        // Act
        var result = await _service.EliminarAsync(tipo.IdTipoUsuario);

        // Assert
        result.Exitoso.Should().BeTrue();
        result.Datos.Should().BeTrue();
        tipo.Activo.Should().BeFalse();
        tipo.FechaBaja.Should().NotBeNull();
        tipo.IdUsuarioUltimaModificacion.Should().Be(5);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
