using FluentAssertions;
using GestionIntegral.Application.DTOs.Emails;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class EmailServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly EmailService _service;

    public EmailServiceTests()
    {
        _service = new EmailService(_unitOfWorkMock.Object, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_NoExiste_LanzaNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.Emails.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Email?)null);

        Func<Task> act = async () => await _service.ObtenerPorIdAsync(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CrearAsync_TipoReferenciaInvalido_LanzaBusinessException()
    {
        var request = new CrearEmailRequestDto { TipoReferencia = "X", IdReferencia = 1, Email = "a@a.com" };

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task CrearAsync_ClienteNoExiste_LanzaNotFoundException()
    {
        var request = new CrearEmailRequestDto { TipoReferencia = "C", IdReferencia = 1, Email = "a@a.com" };
        _unitOfWorkMock.Setup(u => u.Clientes.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Cliente?)null);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task EliminarAsync_Inactivo_LanzaBusinessException()
    {
        _unitOfWorkMock.Setup(u => u.Emails.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Email { IdEmail = 1, Activo = false, DireccionEmail = "a@a.com", TipoReferencia = "C", IdReferencia = 1 });

        Func<Task> act = async () => await _service.EliminarAsync(1);

        await act.Should().ThrowAsync<BusinessException>();
    }
}
