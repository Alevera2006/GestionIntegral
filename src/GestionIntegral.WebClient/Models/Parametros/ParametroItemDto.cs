using System.Text.Json.Serialization;

namespace GestionIntegral.WebClient.Models.Parametros;

public class ParametroItemDto
{
    public int Id => IdProvincia != 0 ? IdProvincia
        : IdTipoComprobante != 0 ? IdTipoComprobante
        : IdMotivoAnulacion != 0 ? IdMotivoAnulacion
        : IdEstadoPedido;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int IdProvincia { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int IdTipoComprobante { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int IdMotivoAnulacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int IdEstadoPedido { get; set; }

    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? CodigoAFIP { get; set; }
    public bool Activo { get; set; }
}
