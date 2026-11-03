using System.Text;
using FluentAssertions;
using GestionIntegral.WebClient.Controllers;
using GestionIntegral.WebClient.Models.Logs;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace GestionIntegral.WebClient.Tests.Controllers;

public class LogsControllerTests : IDisposable
{
    private readonly string _tempPath;
    private readonly Mock<IApiClient> _apiClientMock = new();
    private readonly Mock<IWebHostEnvironment> _environmentMock = new();
    private readonly Mock<ILogger<LogsController>> _loggerMock = new();

    public LogsControllerTests()
    {
        _tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "logs");
        Directory.CreateDirectory(_tempPath);
        _environmentMock.Setup(e => e.ContentRootPath).Returns(Path.GetDirectoryName(_tempPath)!);
    }

    private LogsController CreateController()
    {
        return new LogsController(_apiClientMock.Object, _environmentMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void Index_SinArchivos_DebeRetornarVistaConListaVacia()
    {
        var controller = CreateController();

        var result = controller.Index();

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<LogsViewModel>().Subject;
        model.Archivos.Should().BeEmpty();
        model.ArchivoSeleccionado.Should().BeNull();
    }

    [Fact]
    public void Index_ConArchivos_DebeListarOrdenadosPorFechaDescendente()
    {
        System.IO.File.WriteAllText(Path.Combine(_tempPath, "log-2026-11-01.txt"), "[2026-11-01 10:00:00 INF] Test - Mensaje 1");
        System.IO.File.WriteAllText(Path.Combine(_tempPath, "log-2026-11-03.txt"), "[2026-11-03 10:00:00 INF] Test - Mensaje 3");
        System.IO.File.WriteAllText(Path.Combine(_tempPath, "log-2026-11-02.txt"), "[2026-11-02 10:00:00 INF] Test - Mensaje 2");

        var controller = CreateController();

        var result = controller.Index();

        var model = (result as ViewResult)!.Model as LogsViewModel;
        model!.Archivos.Should().HaveCount(3);
        model.Archivos[0].Nombre.Should().Be("log-2026-11-03.txt");
        model.Archivos[1].Nombre.Should().Be("log-2026-11-02.txt");
        model.Archivos[2].Nombre.Should().Be("log-2026-11-01.txt");
    }

    [Fact]
    public void Index_ConArchivoSeleccionado_DebeParsearEntradas()
    {
        var contenido = new StringBuilder();
        contenido.AppendLine("[2026-11-03 10:00:00 INF] GestionIntegral.WebClient.Services.ApiClient - Mensaje de info");
        contenido.AppendLine("[2026-11-03 10:01:00 ERR] GestionIntegral.WebClient.Controllers.AccountController - Error en login");
        contenido.AppendLine("System.Exception: Fallo algo");
        System.IO.File.WriteAllText(Path.Combine(_tempPath, "log-2026-11-03.txt"), contenido.ToString());

        var controller = CreateController();

        var result = controller.Index("log-2026-11-03.txt");

        var model = (result as ViewResult)!.Model as LogsViewModel;
        model!.ArchivoSeleccionado.Should().NotBeNull();
        model.Entradas.Should().HaveCount(2);
        model.Entradas.Should().Contain(e => e.Nivel == "INF");
        model.Entradas.Should().Contain(e => e.Nivel == "ERR" && e.Excepcion != null && e.Excepcion.Contains("System.Exception"));
    }

    [Fact]
    public void Index_FiltrarPorNivel_DebeRetornarSoloEntradasFiltradas()
    {
        var contenido = "[2026-11-03 10:00:00 INF] Test - Info\n[2026-11-03 10:01:00 ERR] Test - Error\n";
        System.IO.File.WriteAllText(Path.Combine(_tempPath, "log-2026-11-03.txt"), contenido);

        var controller = CreateController();

        var result = controller.Index("log-2026-11-03.txt", nivel: "ERR");

        var model = (result as ViewResult)!.Model as LogsViewModel;
        model!.Entradas.Should().HaveCount(1);
        model.Entradas[0].Nivel.Should().Be("ERR");
    }

    [Fact]
    public void Index_FiltrarPorTexto_DebeRetornarSoloCoincidencias()
    {
        var contenido = "[2026-11-03 10:00:00 INF] Test - Login exitoso\n[2026-11-03 10:01:00 INF] Test - Producto guardado\n";
        System.IO.File.WriteAllText(Path.Combine(_tempPath, "log-2026-11-03.txt"), contenido);

        var controller = CreateController();

        var result = controller.Index("log-2026-11-03.txt", texto: "Login");

        var model = (result as ViewResult)!.Model as LogsViewModel;
        model!.Entradas.Should().HaveCount(1);
        model.Entradas[0].Mensaje.Should().Contain("Login");
    }

    [Fact]
    public void Descargar_ArchivoExistente_DebeRetornarFileResult()
    {
        var ruta = Path.Combine(_tempPath, "log-2026-11-03.txt");
        System.IO.File.WriteAllText(ruta, "contenido");

        var controller = CreateController();

        var result = controller.Descargar("log-2026-11-03.txt");

        var fileResult = result.Should().BeOfType<FileContentResult>().Subject;
        fileResult.FileDownloadName.Should().Be("log-2026-11-03.txt");
        fileResult.ContentType.Should().Be("text/plain");
    }

    [Fact]
    public void Descargar_ArchivoInexistente_DebeRetornarNotFound()
    {
        var controller = CreateController();

        var result = controller.Descargar("no-existe.txt");

        result.Should().BeOfType<NotFoundResult>();
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempPath))
            Directory.Delete(_tempPath, true);
    }
}
