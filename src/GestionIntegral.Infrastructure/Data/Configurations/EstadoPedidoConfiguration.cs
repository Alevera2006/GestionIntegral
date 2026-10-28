using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class EstadoPedidoConfiguration : ConfiguracionBaseConfiguration<EstadoPedido>
{
    public EstadoPedidoConfiguration() : base("EstadosPedido", "IdEstadoPedido") { }

    public override void Configure(EntityTypeBuilder<EstadoPedido> builder)
    {
        base.Configure(builder);
    }
}
