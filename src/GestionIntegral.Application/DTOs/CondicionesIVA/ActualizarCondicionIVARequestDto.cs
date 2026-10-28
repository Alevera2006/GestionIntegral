namespace GestionIntegral.Application.DTOs.CondicionesIVA;

public class ActualizarCondicionIVARequestDto
{
    public int IdCondicionIVA { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
