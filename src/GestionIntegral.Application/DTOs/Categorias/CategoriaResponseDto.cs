namespace GestionIntegral.Application.DTOs.Categorias;

public class CategoriaResponseDto
{
    public int IdCategoria { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
