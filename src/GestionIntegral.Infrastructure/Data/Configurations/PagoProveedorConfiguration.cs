using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class PagoProveedorConfiguration : IEntityTypeConfiguration<PagoProveedor>
{
    public void Configure(EntityTypeBuilder<PagoProveedor> builder)
    {
        builder.ToTable("PagosProveedores");
        builder.HasKey(p => p.IdPagoProveedor);
        builder.Property(p => p.IdPagoProveedor).ValueGeneratedOnAdd();
        builder.Property(p => p.Fecha).HasColumnType("datetime2").IsRequired();
        builder.Property(p => p.Monto).HasColumnType("decimal(18,2)");
        builder.Property(p => p.NumeroCheque).HasMaxLength(50);
        builder.Property(p => p.Banco).HasMaxLength(100);
        builder.Property(p => p.FechaVencimientoCheque).HasColumnType("datetime2");
        builder.Property(p => p.Concepto).HasMaxLength(250).IsRequired();
        builder.Property(p => p.Observaciones).HasMaxLength(500);
        builder.Property(p => p.Activo).IsRequired();

        builder.HasOne(p => p.Proveedor)
            .WithMany()
            .HasForeignKey(p => p.IdProveedor)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.MedioPago)
            .WithMany()
            .HasForeignKey(p => p.IdMedioPago)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
