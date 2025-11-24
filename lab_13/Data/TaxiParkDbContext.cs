using Microsoft.EntityFrameworkCore;
using TaxiParkAppMobile1.Models;

namespace TaxiParkAppMobile1.Data;

public class TaxiParkDbContext : DbContext
{
    public TaxiParkDbContext(DbContextOptions<TaxiParkDbContext> options)
        : base(options)
    {
    }

    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Trip> Trips => Set<Trip>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip>()
            .Property(t => t.Price)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Trip>()
            .HasOne(t => t.Driver)
            .WithMany(d => d.Trips)
            .HasForeignKey(t => t.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Trip>()
            .HasOne(t => t.Car)
            .WithMany(c => c.Trips)
            .HasForeignKey(t => t.CarId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}

