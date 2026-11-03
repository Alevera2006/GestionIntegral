namespace GestionIntegral.WebClient.Models.Logs;

public class LogFileViewModel
{
    public string Nombre { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public long TamanioBytes { get; set; }
    public string TamanioFormateado => TamanioBytes < 1024
        ? $"{TamanioBytes} B"
        : TamanioBytes < 1024 * 1024
            ? $"{TamanioBytes / 1024.0:F2} KB"
            : $"{TamanioBytes / (1024.0 * 1024):F2} MB";
}
