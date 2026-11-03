using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class ValorCarteraConfiguration : IEntityTypeConfiguration<ValorCartera>
{
    public void Configure(EntityTypeBuilder<ValorCartera> builder)
    {
        builder.ToTable("ValoresCartera");
        builder.HasKey(v => v.IdValorCartera);
        builder.Property(v => v.IdValorCartera).ValueGeneratedOnAdd();
        builder.Property(v => v.TipoValor).HasMaxLength(50).IsRequired();
        builder.Property(v => v.Numero).HasMaxLength(50).IsRequired();
        builder.Property(v => v.Banco).HasMaxLength(100);
        builder.Property(v => v.FechaEmision).HasColumnType("datetime2");
        builder.Property(v => v.FechaVencimiento).HasColumnType("datetime2");
        builder.Property(v => v.Importe).HasColumnType("decimal(18,2)");
        builder.Property(v => v.Estado).HasMaxLength(50).IsRequired();
        builder.Property(v => v.Observaciones).HasMaxLength(500);
        builder.Property(v => v.Activo).IsRequired();

        builder.HasOne(v => v.PagoCliente)
            .WithOne(p => p.ValorCartera)
            .HasForeignKey<ValorCartera>(v => v.IdPagoCliente)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(v => v.Cliente)
            .WithMany()
            .HasForeignKey(v => v.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
