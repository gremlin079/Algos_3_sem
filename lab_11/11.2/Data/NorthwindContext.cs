using Microsoft.EntityFrameworkCore;
using Northwind.Api.Models;

namespace Northwind.Api.Data;

public class NorthwindContext(DbContextOptions<NorthwindContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.HasKey(e => e.ProductId);

            entity.Property(e => e.ProductId)
                .HasColumnName("ProductID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(40);
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);
            entity.Property(e => e.UnitPrice)
                .HasDefaultValue(0m);
            entity.Property(e => e.UnitsInStock);
            entity.Property(e => e.UnitsOnOrder);
            entity.Property(e => e.ReorderLevel);
            entity.Property(e => e.Discontinued).HasDefaultValue(false);
        });
    }
}

