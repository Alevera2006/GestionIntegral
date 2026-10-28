using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class SubCategoriaConfiguration : IEntityTypeConfiguration<SubCategoria>
{
    public void Configure(EntityTypeBuilder<SubCategoria> builder)
    {
        builder.ToTable("SubCategorias");

        builder.HasKey(e => e.IdSubCategoria);

        builder.Property(e => e.IdSubCategoria)
            .HasColumnName("IdSubCategoria")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.IdCategoria)
            .HasColumnName("IdCategoria")
            .IsRequired();

        builder.Property(e => e.Codigo)
            .HasColumnName("Codigo")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.Descripcion)
            .HasColumnName("Descripcion")
            .HasMaxLength(100)
            .IsRequired();

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

        builder.HasOne(e => e.Categoria)
            .WithMany(c => c.SubCategorias)
            .HasForeignKey(e => e.IdCategoria)
            .HasConstraintName("FK_SubCategorias_Categorias")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_SubCategorias_UsuariosCreacion")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioModificacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioUltimaModificacion)
            .HasConstraintName("FK_SubCategorias_UsuariosModificacion")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
