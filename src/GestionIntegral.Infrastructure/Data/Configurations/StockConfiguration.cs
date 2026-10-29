using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.ToTable("Stock");

        builder.HasKey(e => e.IdStock);

        builder.Property(e => e.IdStock)
            .HasColumnName("IdStock")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.IdDeposito)
            .HasColumnName("IdDeposito")
            .IsRequired();

        builder.Property(e => e.IdProducto)
            .HasColumnName("IdProducto")
            .IsRequired();

        builder.Property(e => e.Cantidad)
            .HasColumnName("Cantidad")
            .HasColumnType("decimal(18,4)")
            .HasDefaultValue(0m)
            .IsRequired();

        builder.Property(e => e.FechaUltimoMovimiento)
            .HasColumnName("FechaUltimoMovimiento")
            .HasColumnType("datetime2");

        builder.HasIndex(e => new { e.IdDeposito, e.IdProducto })
            .IsUnique()
            .HasDatabaseName("UQ_Stock_DepositoProducto");

        builder.HasOne(e => e.Deposito)
            .WithMany(d => d.Stocks)
            .HasForeignKey(e => e.IdDeposito)
            .HasConstraintName("FK_Stock_Depositos")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Producto)
            .WithMany()
            .HasForeignKey(e => e.IdProducto)
            .HasConstraintName("FK_Stock_Productos")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
