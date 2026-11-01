using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class EmailConfiguration : IEntityTypeConfiguration<Email>
{
    public void Configure(EntityTypeBuilder<Email> builder)
    {
        builder.ToTable("Emails", t =>
        {
            t.HasCheckConstraint("CK_Emails_TipoReferencia", "TipoReferencia IN ('C','P')");
            t.HasCheckConstraint("CK_Emails_IdReferencia", "IdReferencia > 0");
        });

        builder.HasKey(e => e.IdEmail);

        builder.Property(e => e.IdEmail)
            .HasColumnName("IdEmail")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.TipoReferencia)
            .HasColumnName("TipoReferencia")
            .HasMaxLength(1)
            .IsRequired();

        builder.Property(e => e.IdReferencia)
            .HasColumnName("IdReferencia")
            .IsRequired();

        builder.Property(e => e.DireccionEmail)
            .HasColumnName("Email")
            .HasMaxLength(254)
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

        builder.HasIndex(e => new { e.TipoReferencia, e.IdReferencia }).HasDatabaseName("IX_Emails_TipoReferencia_IdReferencia");

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_Emails_UsuariosCreacion")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioModificacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioUltimaModificacion)
            .HasConstraintName("FK_Emails_UsuariosModificacion")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
