using Microsoft.EntityFrameworkCore;
using AirlineService.Models;

namespace AirlineService.Data
{
    public class AirlineServiceDbContext : DbContext
    {
        public AirlineServiceDbContext(DbContextOptions<AirlineServiceDbContext> options) : base(options)
        {
        }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
            .HasKey(t => new { t.FlightId, t.PassengerId });

            modelBuilder.Entity<Booking>()
                .HasOne(pt => pt.Flight)
                .WithMany(p => p.Passengers)
                .HasForeignKey(pt => pt.FlightId);

            modelBuilder.Entity<Booking>()
                .HasOne(pt => pt.Passenger)
                .WithMany(t => t.Flights)
                .HasForeignKey(pt => pt.PassengerId);
        }
        public DbSet<AirlineService.Models.Booking>? Booking { get; set; }
    }
}
