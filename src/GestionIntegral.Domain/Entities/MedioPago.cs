namespace GestionIntegral.Domain.Entities;

public class MedioPago : EntityBase
{
    public int IdMedioPago { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool EsEfectivo { get; set; }
    public bool RequiereReferencia { get; set; }
    public bool Activo { get; set; } = true;

    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual ICollection<CajaDetalle> CajaDetalles { get; set; } = new List<CajaDetalle>();
}
