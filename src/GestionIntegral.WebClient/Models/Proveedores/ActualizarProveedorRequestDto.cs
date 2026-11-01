namespace GestionIntegral.WebClient.Models.Proveedores;

public class ActualizarProveedorRequestDto : CrearProveedorRequestDto
{
    public int IdProveedor { get; set; }
    public bool Activo { get; set; }
}
