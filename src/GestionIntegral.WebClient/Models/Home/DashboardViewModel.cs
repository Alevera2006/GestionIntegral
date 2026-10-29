namespace GestionIntegral.WebClient.Models.Home;

public class DashboardViewModel
{
    public string UsuarioNombre { get; set; } = string.Empty;
    public int TotalUsuarios { get; set; }
    public int UsuariosActivos { get; set; }
    public int TotalTiposUsuario { get; set; }
    public int TotalParametros { get; set; }
    public List<AccesoRapidoViewModel> AccesosRapidos { get; set; } = new();
}

public class AccesoRapidoViewModel
{
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Icono { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
