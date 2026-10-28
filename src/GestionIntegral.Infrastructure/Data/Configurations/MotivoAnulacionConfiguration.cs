using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class MotivoAnulacionConfiguration : ConfiguracionBaseConfiguration<MotivoAnulacion>
{
    public MotivoAnulacionConfiguration() : base("MotivosAnulacion", "IdMotivoAnulacion", 200) { }

    public override void Configure(EntityTypeBuilder<MotivoAnulacion> builder)
    {
        base.Configure(builder);
    }
}
