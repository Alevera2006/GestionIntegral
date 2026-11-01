namespace GestionIntegral.Application.DTOs.Domicilios;

public class ActualizarDomicilioRequestDto : CrearDomicilioRequestDto
{
    public int IdDomicilio { get; set; }
    public bool Activo { get; set; }
}
