using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class MovimientoStockDetalleConfiguration : IEntityTypeConfiguration<MovimientoStockDetalle>
{
    public void Configure(EntityTypeBuilder<MovimientoStockDetalle> builder)
    {
        builder.ToTable("MovimientoStockDetalle");

        builder.HasKey(e => e.IdMovimientoStockDetalle);

        builder.Property(e => e.IdMovimientoStockDetalle)
            .HasColumnName("IdMovimientoStockDetalle")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.IdMovimientoStock)
            .HasColumnName("IdMovimientoStock")
            .IsRequired();

        builder.Property(e => e.IdProducto)
            .HasColumnName("IdProducto")
            .IsRequired();

        builder.Property(e => e.Cantidad)
            .HasColumnName("Cantidad")
            .HasColumnType("decimal(18,4)")
            .IsRequired();

        builder.Property(e => e.IdUsuarioCreacion)
            .HasColumnName("IdUsuarioCreacion")
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.HasOne(e => e.MovimientoStock)
            .WithMany(m => m.Detalles)
            .HasForeignKey(e => e.IdMovimientoStock)
            .HasConstraintName("FK_MovimientoStockDetalle_Movimiento")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Producto)
            .WithMany()
            .HasForeignKey(e => e.IdProducto)
            .HasConstraintName("FK_MovimientoStockDetalle_Producto")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_MovimientoStockDetalle_Usuario")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
