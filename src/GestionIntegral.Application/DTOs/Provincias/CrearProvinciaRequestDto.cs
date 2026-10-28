namespace GestionIntegral.Application.DTOs.Provincias;

public class CrearProvinciaRequestDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? CodigoAFIP { get; set; }
}
