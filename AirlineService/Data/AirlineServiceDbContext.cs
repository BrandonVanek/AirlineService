using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AirlineService.Models;

namespace AirlineService.Data
{
    public class AirlineServiceDbContext : DbContext
    {
        public AirlineServiceDbContext(DbContextOptions<AirlineServiceDbContext> options) : base(options) { }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Booking>()
                .HasOne(pt => pt.Flight)
                .WithMany(p => p.Bookings)
                .HasForeignKey(pt => pt.FlightId);

            modelBuilder.Entity<Booking>()
                .HasOne(pt => pt.Passenger)
                .WithMany(t => t.Bookings)
                .HasForeignKey(pt => pt.PassengerId);
        }
    }
}
