namespace GestionIntegral.Application.DTOs.Telefonos;

public class ActualizarTelefonoRequestDto : CrearTelefonoRequestDto
{
    public int IdTelefono { get; set; }
    public bool Activo { get; set; }
}
