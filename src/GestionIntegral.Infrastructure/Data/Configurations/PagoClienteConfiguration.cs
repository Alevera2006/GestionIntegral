using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class PagoClienteConfiguration : IEntityTypeConfiguration<PagoCliente>
{
    public void Configure(EntityTypeBuilder<PagoCliente> builder)
    {
        builder.ToTable("PagosClientes");
        builder.HasKey(p => p.IdPagoCliente);
        builder.Property(p => p.IdPagoCliente).ValueGeneratedOnAdd();
        builder.Property(p => p.Fecha).HasColumnType("datetime2").IsRequired();
        builder.Property(p => p.Monto).HasColumnType("decimal(18,2)");
        builder.Property(p => p.NumeroCheque).HasMaxLength(50);
        builder.Property(p => p.Banco).HasMaxLength(100);
        builder.Property(p => p.FechaVencimientoCheque).HasColumnType("datetime2");
        builder.Property(p => p.Observaciones).HasMaxLength(500);
        builder.Property(p => p.Activo).IsRequired();

        builder.HasOne(p => p.Cliente)
            .WithMany()
            .HasForeignKey(p => p.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Factura)
            .WithMany(f => f.PagosAplicados)
            .HasForeignKey(p => p.IdFactura)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.MedioPago)
            .WithMany()
            .HasForeignKey(p => p.IdMedioPago)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
