using Microsoft.EntityFrameworkCore;
using TaxiCompanyApp.Models;

namespace TaxiCompanyApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация для decimal поля
            modelBuilder.Entity<Trip>()
                .Property(t => t.Price)
                .HasPrecision(10, 2);

            // Дополнительная конфигурация отношений
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Driver)
                .WithMany(d => d.Trips)
                .HasForeignKey(t => t.DriverId);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Car)
                .WithMany(c => c.Trips)
                .HasForeignKey(t => t.CarId);
        }
    }
}