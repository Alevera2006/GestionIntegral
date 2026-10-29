namespace GestionIntegral.WebClient.Models.Parametros;

public class TarjetaParametroViewModel
{
    public string Key { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Icono { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;
    public bool RequiereCodigoAFIP { get; set; }
}
