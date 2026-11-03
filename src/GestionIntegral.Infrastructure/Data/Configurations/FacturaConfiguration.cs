using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class FacturaConfiguration : IEntityTypeConfiguration<Factura>
{
    public void Configure(EntityTypeBuilder<Factura> builder)
    {
        builder.ToTable("Facturas");
        builder.HasKey(f => f.IdFactura);
        builder.Property(f => f.IdFactura).ValueGeneratedOnAdd();
        builder.Property(f => f.NumeroComprobante).HasMaxLength(20).IsRequired();
        builder.Property(f => f.NombreCliente).HasMaxLength(150).IsRequired();
        builder.Property(f => f.Fecha).HasColumnType("datetime2").IsRequired();
        builder.Property(f => f.Subtotal).HasColumnType("decimal(18,2)");
        builder.Property(f => f.ImporteIva).HasColumnType("decimal(18,2)");
        builder.Property(f => f.Total).HasColumnType("decimal(18,2)");
        builder.Property(f => f.Observaciones).HasMaxLength(500);
        builder.Property(f => f.Activo).IsRequired();
        builder.Property(f => f.FechaBaja).HasColumnType("datetime2");

        builder.HasOne(f => f.Cliente)
            .WithMany()
            .HasForeignKey(f => f.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.TipoComprobante)
            .WithMany()
            .HasForeignKey(f => f.IdTipoComprobante)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.CondicionPago)
            .WithMany()
            .HasForeignKey(f => f.IdCondicionPago)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
