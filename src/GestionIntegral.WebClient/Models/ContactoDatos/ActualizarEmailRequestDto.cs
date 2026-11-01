namespace GestionIntegral.WebClient.Models.ContactoDatos;

public class ActualizarEmailRequestDto : CrearEmailRequestDto
{
    public int IdEmail { get; set; }
    public bool Activo { get; set; }
}
