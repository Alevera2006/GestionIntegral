namespace GestionIntegral.Application.DTOs.SubCategorias;

public class ActualizarSubCategoriaRequestDto
{
    public int IdSubCategoria { get; set; }
    public int IdCategoria { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
