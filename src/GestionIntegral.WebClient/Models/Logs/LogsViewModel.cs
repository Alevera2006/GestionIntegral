namespace GestionIntegral.WebClient.Models.Logs;

public class LogsViewModel
{
    public List<LogFileViewModel> Archivos { get; set; } = new();
    public LogFileViewModel? ArchivoSeleccionado { get; set; }
    public List<LogEntryViewModel> Entradas { get; set; } = new();
    public string? FiltroNivel { get; set; }
    public string? FiltroTexto { get; set; }
    public DateTime? FiltroFechaDesde { get; set; }
    public DateTime? FiltroFechaHasta { get; set; }
    public List<string> NivelesDisponibles { get; set; } = new() { "INF", "WRN", "ERR", "DBG", "FTL" };
}
