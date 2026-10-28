using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class EstadoProcesoConfiguration : ConfiguracionBaseConfiguration<EstadoProceso>
{
    public EstadoProcesoConfiguration() : base("EstadosProceso", "IdEstadoProceso") { }

    public override void Configure(EntityTypeBuilder<EstadoProceso> builder)
    {
        base.Configure(builder);
    }
}
