namespace GestionIntegral.WebClient.Models.Parametros;

public class AlicuotaIvaResponseDto
{
    public int IdAlicuotaImpuesto { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Porcentaje { get; set; }
    public bool Activo { get; set; }
}
