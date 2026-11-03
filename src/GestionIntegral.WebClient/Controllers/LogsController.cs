using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using GestionIntegral.WebClient.Models.Logs;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class LogsController : BaseController
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<LogsController> _logger;

    public LogsController(IApiClient apiClient, IWebHostEnvironment environment, ILogger<LogsController> logger) : base(apiClient)
    {
        _environment = environment;
        _logger = logger;
    }

    public IActionResult Index(string? archivo = null, string? nivel = null, string? texto = null,
        DateTime? fechaDesde = null, DateTime? fechaHasta = null)
    {
        var vm = new LogsViewModel
        {
            FiltroNivel = nivel,
            FiltroTexto = texto,
            FiltroFechaDesde = fechaDesde,
            FiltroFechaHasta = fechaHasta
        };

        var logPath = ObtenerRutaLogs();
        if (!Directory.Exists(logPath))
        {
            Directory.CreateDirectory(logPath);
        }

        vm.Archivos = Directory
            .GetFiles(logPath, "log-*.txt")
            .Select(f => new FileInfo(f))
            .OrderByDescending(f => f.Name)
            .Select(f => new LogFileViewModel
            {
                Nombre = f.Name,
                NombreCompleto = f.FullName,
                Fecha = f.LastWriteTime,
                TamanioBytes = f.Length
            })
            .ToList();

        if (!string.IsNullOrWhiteSpace(archivo))
        {
            var archivoSeleccionado = vm.Archivos.FirstOrDefault(a => a.Nombre.Equals(archivo, StringComparison.OrdinalIgnoreCase));
            if (archivoSeleccionado != null)
            {
                vm.ArchivoSeleccionado = archivoSeleccionado;
                vm.Entradas = LeerEntradas(archivoSeleccionado.NombreCompleto);
                vm.Entradas = AplicarFiltros(vm.Entradas, nivel, texto, fechaDesde, fechaHasta);
            }
        }

        return View(vm);
    }

    public IActionResult Descargar(string archivo)
    {
        var logPath = ObtenerRutaLogs();
        var filePath = Path.Combine(logPath, archivo);

        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var content = System.IO.File.ReadAllBytes(filePath);
        return File(content, "text/plain", archivo);
    }

    private string ObtenerRutaLogs()
    {
        return Path.Combine(_environment.ContentRootPath, "logs");
    }

    private static List<LogEntryViewModel> LeerEntradas(string filePath)
    {
        var entradas = new List<LogEntryViewModel>();
        if (!System.IO.File.Exists(filePath)) return entradas;

        var lineas = System.IO.File.ReadAllLines(filePath, Encoding.UTF8);
        var regex = new Regex(@"^\[(?<fecha>\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2})\s(?<nivel>\w+)\]\s(?<fuente>[^-]+)\s-\s(?<mensaje>.*)$", RegexOptions.Compiled);

        LogEntryViewModel? entradaActual = null;

        foreach (var linea in lineas)
        {
            var match = regex.Match(linea);
            if (match.Success)
            {
                entradaActual = new LogEntryViewModel
                {
                    Fecha = DateTime.ParseExact(match.Groups["fecha"].Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    Nivel = match.Groups["nivel"].Value.Trim(),
                    Fuente = match.Groups["fuente"].Value.Trim(),
                    Mensaje = match.Groups["mensaje"].Value.Trim(),
                    LineaOriginal = linea
                };
                entradas.Add(entradaActual);
            }
            else if (entradaActual != null)
            {
                entradaActual.Excepcion = string.IsNullOrEmpty(entradaActual.Excepcion)
                    ? linea
                    : entradaActual.Excepcion + Environment.NewLine + linea;
                entradaActual.LineaOriginal += Environment.NewLine + linea;
            }
        }

        return entradas.OrderByDescending(e => e.Fecha).ToList();
    }

    private static List<LogEntryViewModel> AplicarFiltros(List<LogEntryViewModel> entradas, string? nivel, string? texto,
        DateTime? fechaDesde, DateTime? fechaHasta)
    {
        if (!string.IsNullOrWhiteSpace(nivel))
        {
            entradas = entradas.Where(e => e.Nivel.Equals(nivel, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(texto))
        {
            var filtroLower = texto.ToLowerInvariant();
            entradas = entradas.Where(e =>
                e.Mensaje.ToLowerInvariant().Contains(filtroLower) ||
                e.Fuente.ToLowerInvariant().Contains(filtroLower) ||
                (e.Excepcion?.ToLowerInvariant().Contains(filtroLower) ?? false))
                .ToList();
        }

        if (fechaDesde.HasValue)
        {
            entradas = entradas.Where(e => e.Fecha >= fechaDesde.Value).ToList();
        }

        if (fechaHasta.HasValue)
        {
            entradas = entradas.Where(e => e.Fecha <= fechaHasta.Value.AddDays(1).AddTicks(-1)).ToList();
        }

        return entradas;
    }
}
