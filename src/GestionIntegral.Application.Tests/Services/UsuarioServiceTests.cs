using AutoMapper;
using FluentAssertions;
using GestionIntegral.Application.DTOs.Usuarios;
using GestionIntegral.Application.Mappings;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class UsuarioServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly IMapper _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
    private readonly UsuarioService _service;

    public UsuarioServiceTests()
    {
        _service = new UsuarioService(_unitOfWorkMock.Object, _mapper, _passwordServiceMock.Object, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task CrearAsync_UsuarioExistente_LanzaBusinessException()
    {
        var request = new CrearUsuarioRequestDto { UsuarioNombre = "admin", Nombre = "A", Apellido = "B", Clave = "123", IdTipoUsuario = 1 };
        _unitOfWorkMock.Setup(u => u.Usuarios.ExisteUsuarioAsync(request.UsuarioNombre, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task CrearAsync_TipoUsuarioNoExiste_LanzaNotFoundException()
    {
        var request = new CrearUsuarioRequestDto { UsuarioNombre = "admin", Nombre = "A", Apellido = "B", Clave = "123", IdTipoUsuario = 1 };
        _unitOfWorkMock.Setup(u => u.Usuarios.ExisteUsuarioAsync(request.UsuarioNombre, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.TiposUsuario.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((TipoUsuario?)null);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_DatosValidos_CreaUsuario()
    {
        var request = new CrearUsuarioRequestDto { UsuarioNombre = "admin", Nombre = "A", Apellido = "B", Clave = "123", IdTipoUsuario = 1 };
        _unitOfWorkMock.Setup(u => u.Usuarios.ExisteUsuarioAsync(request.UsuarioNombre, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.TiposUsuario.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new TipoUsuario { IdTipoUsuario = 1, Descripcion = "Admin" });
        _passwordServiceMock.Setup(p => p.Hash("123")).Returns("HASH");

        var result = await _service.CrearAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.UsuarioNombre.Should().Be("admin");
    }

    [Fact]
    public async Task EliminarAsync_Inactivo_LanzaBusinessException()
    {
        _unitOfWorkMock.Setup(u => u.Usuarios.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Usuario { IdUsuario = 1, Activo = false });

        Func<Task> act = async () => await _service.EliminarAsync(1);

        await act.Should().ThrowAsync<BusinessException>();
    }
}
