namespace GestionIntegral.WebClient.Models.Logs;

public class LogEntryViewModel
{
    public DateTime Fecha { get; set; }
    public string Nivel { get; set; } = string.Empty;
    public string Fuente { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public string? Excepcion { get; set; }
    public string LineaOriginal { get; set; } = string.Empty;
}
