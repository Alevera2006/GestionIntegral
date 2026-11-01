namespace GestionIntegral.Domain.Entities;

public class Domicilio : EntityBase
{
    public int IdDomicilio { get; set; }
    public string TipoReferencia { get; set; } = string.Empty;
    public int IdReferencia { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string? Localidad { get; set; }
    public int IdProvincia { get; set; }
    public string? CodigoPostal { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime? FechaBaja { get; set; }

    public virtual Provincia Provincia { get; set; } = null!;
    public virtual Usuario UsuarioCreacion { get; set; } = null!;
    public virtual Usuario? UsuarioModificacion { get; set; }
}
