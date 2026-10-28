using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class EstadoCompraConfiguration : ConfiguracionBaseConfiguration<EstadoCompra>
{
    public EstadoCompraConfiguration() : base("EstadosCompra", "IdEstadoCompra") { }

    public override void Configure(EntityTypeBuilder<EstadoCompra> builder)
    {
        base.Configure(builder);
    }
}
