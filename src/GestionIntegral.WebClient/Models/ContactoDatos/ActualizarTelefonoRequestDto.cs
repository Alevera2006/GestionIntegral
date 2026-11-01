namespace GestionIntegral.WebClient.Models.ContactoDatos;

public class ActualizarTelefonoRequestDto : CrearTelefonoRequestDto
{
    public int IdTelefono { get; set; }
    public bool Activo { get; set; }
}
