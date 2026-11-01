namespace GestionIntegral.WebClient.Models.ContactoDatos;

public class ActualizarDomicilioRequestDto : CrearDomicilioRequestDto
{
    public int IdDomicilio { get; set; }
    public bool Activo { get; set; }
}
