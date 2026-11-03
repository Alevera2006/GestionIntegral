using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class FacturaDetalleConfiguration : IEntityTypeConfiguration<FacturaDetalle>
{
    public void Configure(EntityTypeBuilder<FacturaDetalle> builder)
    {
        builder.ToTable("FacturasDetalle");
        builder.HasKey(fd => fd.IdFacturaDetalle);
        builder.Property(fd => fd.IdFacturaDetalle).ValueGeneratedOnAdd();
        builder.Property(fd => fd.Descripcion).HasMaxLength(150).IsRequired();
        builder.Property(fd => fd.Cantidad).HasColumnType("decimal(18,4)");
        builder.Property(fd => fd.PrecioUnitario).HasColumnType("decimal(18,4)");
        builder.Property(fd => fd.PorcentajeIva).HasColumnType("decimal(18,2)");
        builder.Property(fd => fd.Subtotal).HasColumnType("decimal(18,2)");
        builder.Property(fd => fd.ImporteIva).HasColumnType("decimal(18,2)");

        builder.HasOne(fd => fd.Factura)
            .WithMany(f => f.Detalles)
            .HasForeignKey(fd => fd.IdFactura)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(fd => fd.Producto)
            .WithMany()
            .HasForeignKey(fd => fd.IdProducto)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
