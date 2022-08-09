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
        public class FlightConfiguration : IEntityTypeConfiguration<Flight>
        {
            public void Configure(EntityTypeBuilder<Flight> builder)
            {
                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.FlightNumber).HasMaxLength(64).IsUnicode().IsRequired();

                builder.HasKey(e => e.Id);
                builder.HasIndex(e => e.FlightNumber).IsUnique();
            }
        }

        public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
        {
            public void Configure(EntityTypeBuilder<Passenger> builder)
            {
                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.Name).HasMaxLength(64).IsUnicode().IsRequired();

                builder.HasKey(e => e.Id);
                builder.HasIndex(e => e.Name).IsUnique(false);
            }
        }

        public class BookingConfiguration : IEntityTypeConfiguration<Booking>
        {
            public void Configure(EntityTypeBuilder<Booking> builder)
            {
                builder.Property(e => e.FlightId).IsRequired();
                builder.Property(e => e.PassengerId).IsRequired();

                builder.HasIndex(e => new { e.FlightId, e.PassengerId }).IsUnique();
                builder.HasKey(t => new { t.FlightId, t.PassengerId });

                //builder.HasOne(e => e.Flight).WithMany(e => e.Passengers).HasForeignKey(e => e.FlightId);
                //builder.HasOne(e => e.Passenger).WithMany(e => e.Flights).HasForeignKey(e => e.PassengerId);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new FlightConfiguration());
            modelBuilder.ApplyConfiguration(new PassengerConfiguration());
            modelBuilder.ApplyConfiguration(new BookingConfiguration());

            //modelBuilder.Entity<Booking>()
            //    .HasOne(pt => pt.Flight)
            //    .WithMany(p => p.Passengers)
            //    .HasForeignKey(pt => pt.FlightId);

            //modelBuilder.Entity<Booking>()
            //    .HasOne(pt => pt.Passenger)
            //    .WithMany(t => t.Flights)
            //    .HasForeignKey(pt => pt.PassengerId);

            //modelBuilder.Entity<Flight>()
            //.HasMany(p => p.Passengers)
            //.WithMany(p => p.Flights)
            //.UsingEntity<Booking>(
            //    j => j
            //        .HasOne(pt => pt.Passenger)
            //        .WithMany(t => t.Bookings)
            //        .HasForeignKey(pt => pt.PassengerId),
            //    j => j
            //        .HasOne(pt => pt.Flight)
            //        .WithMany(p => p.Bookings)
            //        .HasForeignKey(pt => pt.FlightId),
            //    j => j
            //        .HasKey(t => new { t.FlightId, t.PassengerId }));
        }
    }
}
