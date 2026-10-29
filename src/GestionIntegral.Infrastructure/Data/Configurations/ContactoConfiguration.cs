using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class ContactoConfiguration : IEntityTypeConfiguration<Contacto>
{
    public void Configure(EntityTypeBuilder<Contacto> builder)
    {
        builder.ToTable("Contactos");

        builder.HasKey(e => e.IdContacto);

        builder.Property(e => e.IdContacto)
            .HasColumnName("IdContacto")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Nombre)
            .HasColumnName("Nombre")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Apellido)
            .HasColumnName("Apellido")
            .HasMaxLength(100);

        builder.Property(e => e.Cargo)
            .HasColumnName("Cargo")
            .HasMaxLength(100);

        builder.Property(e => e.Telefono)
            .HasColumnName("Telefono")
            .HasMaxLength(50);

        builder.Property(e => e.Celular)
            .HasColumnName("Celular")
            .HasMaxLength(50);

        builder.Property(e => e.Email)
            .HasColumnName("Email")
            .HasMaxLength(150);

        builder.Property(e => e.Activo)
            .HasColumnName("Activo")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(e => e.IdUsuarioCreacion)
            .HasColumnName("IdUsuarioCreacion")
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_Contactos_Usuarios")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
