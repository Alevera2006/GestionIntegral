namespace GestionIntegral.Application.DTOs.ConfiguracionBase;

public abstract class ConfiguracionResponseDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
