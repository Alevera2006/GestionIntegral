using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class TipoComprobanteConfiguration : ConfiguracionBaseConfiguration<TipoComprobante>
{
    public TipoComprobanteConfiguration() : base("TiposComprobante", "IdTipoComprobante") { }

    public override void Configure(EntityTypeBuilder<TipoComprobante> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.CodigoAFIP)
            .HasColumnName("CodigoAFIP")
            .HasMaxLength(10);
    }
}
