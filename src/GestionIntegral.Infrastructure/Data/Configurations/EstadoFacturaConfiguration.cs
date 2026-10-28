using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class EstadoFacturaConfiguration : ConfiguracionBaseConfiguration<EstadoFactura>
{
    public EstadoFacturaConfiguration() : base("EstadosFactura", "IdEstadoFactura") { }

    public override void Configure(EntityTypeBuilder<EstadoFactura> builder)
    {
        base.Configure(builder);
    }
}
