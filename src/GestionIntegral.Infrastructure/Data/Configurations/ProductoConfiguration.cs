using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("Productos");

        builder.HasKey(e => e.IdProducto);

        builder.Property(e => e.IdProducto)
            .HasColumnName("IdProducto")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Codigo)
            .HasColumnName("Codigo")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Descripcion)
            .HasColumnName("Descripcion")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.IdCategoria)
            .HasColumnName("IdCategoria")
            .IsRequired();

        builder.Property(e => e.IdSubCategoria)
            .HasColumnName("IdSubCategoria");

        builder.Property(e => e.CodigoBarras)
            .HasColumnName("CodigoBarras")
            .HasMaxLength(50);

        builder.Property(e => e.Costo)
            .HasColumnName("Costo")
            .HasColumnType("decimal(18,4)");

        builder.Property(e => e.StockMinimo)
            .HasColumnName("StockMinimo")
            .HasColumnType("decimal(18,4)");

        builder.Property(e => e.StockMaximo)
            .HasColumnName("StockMaximo")
            .HasColumnType("decimal(18,4)");

        builder.Property(e => e.Observaciones)
            .HasColumnName("Observaciones")
            .HasMaxLength(500);

        builder.Property(e => e.Activo)
            .HasColumnName("Activo")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(e => e.FechaBaja)
            .HasColumnName("FechaBaja")
            .HasColumnType("datetime2");

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

        builder.HasIndex(e => e.Codigo)
            .IsUnique()
            .HasDatabaseName("UQ_Productos_Codigo");

        builder.HasOne(e => e.Categoria)
            .WithMany(c => c.Productos)
            .HasForeignKey(e => e.IdCategoria)
            .HasConstraintName("FK_Productos_Categorias")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.SubCategoria)
            .WithMany(s => s.Productos)
            .HasForeignKey(e => e.IdSubCategoria)
            .HasConstraintName("FK_Productos_SubCategorias")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_Productos_UsuariosCreacion")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioModificacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioUltimaModificacion)
            .HasConstraintName("FK_Productos_UsuariosModificacion")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
