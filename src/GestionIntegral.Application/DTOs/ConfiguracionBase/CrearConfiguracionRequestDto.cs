namespace GestionIntegral.Application.DTOs.ConfiguracionBase;

public abstract class CrearConfiguracionRequestDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}
