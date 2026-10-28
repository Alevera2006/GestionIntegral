namespace GestionIntegral.Domain.Entities;

public class Moneda : EntityBase
{
    public int IdMoneda { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? Simbolo { get; set; }
    public byte CantidadDecimales { get; set; } = 2;
    public bool Activo { get; set; } = true;

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual ICollection<CajaDetalle> CajaDetalles { get; set; } = new List<CajaDetalle>();
}
