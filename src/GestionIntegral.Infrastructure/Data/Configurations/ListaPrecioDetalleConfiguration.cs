using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class ListaPrecioDetalleConfiguration : IEntityTypeConfiguration<ListaPrecioDetalle>
{
    public void Configure(EntityTypeBuilder<ListaPrecioDetalle> builder)
    {
        builder.ToTable("ListaPreciosDetalle");

        builder.HasKey(e => e.IdListaPrecioDetalle);

        builder.Property(e => e.IdListaPrecioDetalle)
            .HasColumnName("IdListaPrecioDetalle")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.IdListaPrecio)
            .HasColumnName("IdListaPrecio")
            .IsRequired();

        builder.Property(e => e.IdProducto)
            .HasColumnName("IdProducto")
            .IsRequired();

        builder.Property(e => e.Precio)
            .HasColumnName("Precio")
            .HasColumnType("decimal(18,4)")
            .IsRequired();

        builder.Property(e => e.IdUsuarioCreacion)
            .HasColumnName("IdUsuarioCreacion")
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(e => e.IdUsuarioUltimaModificacion)
            .HasColumnName("IdUsuarioUltimaModificacion");

        builder.Property(e => e.FechaUltimaModificacion)
            .HasColumnName("FechaUltimaModificacion")
            .HasColumnType("datetime2");

        builder.HasIndex(e => new { e.IdListaPrecio, e.IdProducto })
            .IsUnique()
            .HasDatabaseName("UQ_ListaPreciosDetalle");

        builder.HasOne(e => e.ListaPrecio)
            .WithMany(l => l.Detalles)
            .HasForeignKey(e => e.IdListaPrecio)
            .HasConstraintName("FK_ListaPreciosDetalle_ListasPrecios")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Producto)
            .WithMany(p => p.ListaPrecioDetalles)
            .HasForeignKey(e => e.IdProducto)
            .HasConstraintName("FK_ListaPreciosDetalle_Productos")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_ListaPreciosDetalle_UsuariosCreacion")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioModificacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioUltimaModificacion)
            .HasConstraintName("FK_ListaPreciosDetalle_UsuariosModificacion")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
