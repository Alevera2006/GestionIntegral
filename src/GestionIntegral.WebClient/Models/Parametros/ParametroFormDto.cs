namespace GestionIntegral.WebClient.Models.Parametros;

public class ParametroFormDto
{
    public int? Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? CodigoAFIP { get; set; }
    public bool Activo { get; set; } = true;
    public int? IdCategoria { get; set; }
    public int? IdTipoImpuesto { get; set; }
    public decimal? Porcentaje { get; set; }
    public short? Signo { get; set; }
}
