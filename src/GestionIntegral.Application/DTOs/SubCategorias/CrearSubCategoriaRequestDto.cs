namespace GestionIntegral.Application.DTOs.SubCategorias;

public class CrearSubCategoriaRequestDto
{
    public int IdCategoria { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}
