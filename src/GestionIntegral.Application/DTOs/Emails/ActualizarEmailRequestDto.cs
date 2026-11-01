namespace GestionIntegral.Application.DTOs.Emails;

public class ActualizarEmailRequestDto : CrearEmailRequestDto
{
    public int IdEmail { get; set; }
    public bool Activo { get; set; }
}
