namespace GestionIntegral.WebClient.Models.Parametros;

public class TipoComprobanteResponseDto
{
    public int IdTipoComprobante { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? CodigoAFIP { get; set; }
    public bool Activo { get; set; }
}
