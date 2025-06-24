using Microsoft.EntityFrameworkCore;
using BusTicketDemoApp.Models;
using BusTicketDemoApp.Extensions;

namespace BusTicketDemoApp.Data
{
    public class BusTicketDbContext : DbContext
    {
        public BusTicketDbContext(DbContextOptions<BusTicketDbContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<BusSchedule> BusSchedules { get; set; }
        public DbSet<BusBooking> BusBookings { get; set; }
        public DbSet<BusBookingPassenger> BusBookingPassengers { get; set; }
        public DbSet<BusLocation> BusLocations { get; set; }
        public DbSet<Vendor> Vendors { get; set; }        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.HasIndex(e => e.EmailId).IsUnique();
                entity.Property(e => e.Password).HasMaxLength(255);
            });
            
            // Configure BusSchedule entity
            modelBuilder.Entity<BusSchedule>(entity =>
            {
                entity.HasKey(e => e.ScheduleId);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
                entity.HasIndex(e => new { e.FromLocation, e.ToLocation, e.ScheduleDate });
            });
            
            // Configure BusBooking entity
            modelBuilder.Entity<BusBooking>(entity =>
            {
                entity.HasKey(e => e.BookingId);
                entity.HasMany(b => b.BusBookingPassengers)
                    .WithOne(p => p.BusBooking)
                    .HasForeignKey(p => p.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configure BusBookingPassenger entity
            modelBuilder.Entity<BusBookingPassenger>(entity =>
            {
                entity.HasKey(e => e.PassengerId);
                entity.HasIndex(e => new { e.BookingId, e.SeatNo }).IsUnique();
            });
            
            // Configure BusLocation entity
            modelBuilder.Entity<BusLocation>(entity =>
            {
                entity.HasKey(e => e.LocationId);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.LocationName).HasMaxLength(100);
                entity.Property(e => e.Code).HasMaxLength(10);
            });
            
            // Configure Vendor entity
            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasKey(e => e.VendorId);
                entity.Property(e => e.VendorName).HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
            });
            
            // Seed data
            SeedData(modelBuilder);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Locations
            modelBuilder.Entity<BusLocation>().HasData(
                new BusLocation { LocationId = 1, LocationName = "Ciudad de México", Code = "CDMX" },
                new BusLocation { LocationId = 2, LocationName = "Guadalajara", Code = "GDL" },
                new BusLocation { LocationId = 3, LocationName = "Monterrey", Code = "MTY" },
                new BusLocation { LocationId = 4, LocationName = "Puebla", Code = "PUE" },
                new BusLocation { LocationId = 5, LocationName = "Cancún", Code = "CUN" }
            );
            
            // Seed Vendors
            modelBuilder.Entity<Vendor>().HasData(
                new Vendor 
                { 
                    VendorId = 1, 
                    VendorName = "Autobuses del Norte", 
                    Email = "info@autobusdelnorte.com", 
                    Phone = "+52-55-1234-5678",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Vendor 
                { 
                    VendorId = 2, 
                    VendorName = "Primera Plus", 
                    Email = "contacto@primeraplus.com", 
                    Phone = "+52-33-9876-5432",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
                
            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    UserName = "admin",
                    EmailId = "admin@busticket.com",
                    FullName = "Administrador del Sistema",
                    Role = "Admin",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Password = "admin123", // En producción usar hash
                    ProjectName = "BusTicketDemo"
                },
                new User
                {
                    UserId = 2,
                    UserName = "customer1",
                    EmailId = "customer1@email.com",
                    FullName = "Juan Pérez",
                    Role = "Customer",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Password = "customer123", // En producción usar hash
                    ProjectName = "BusTicketDemo"
                },
                new User
                {
                    UserId = 3,
                    UserName = "vendor1",
                    EmailId = "vendor1@email.com",
                    FullName = "María González",
                    Role = "Vendor",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Password = "vendor123", // En producción usar hash
                    ProjectName = "BusTicketDemo"
                }
            );
            
            // Seed Bus Schedules
            var baseDate = new DateTime(2025, 6, 23, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<BusSchedule>().HasData(
                new BusSchedule
                {
                    ScheduleId = 1,
                    VendorId = 1,
                    BusName = "Express CDMX-GDL",
                    BusVehicleNo = "ADN-001",
                    FromLocation = 1, // CDMX
                    ToLocation = 2, // Guadalajara
                    DepartureTime = baseDate.AddHours(8),
                    ArrivalTime = baseDate.AddHours(14),
                    ScheduleDate = baseDate,
                    Price = 450.00m,
                    TotalSeats = 42
                },
                new BusSchedule
                {
                    ScheduleId = 2,
                    VendorId = 1,
                    BusName = "Ejecutivo CDMX-MTY",
                    BusVehicleNo = "ADN-002",
                    FromLocation = 1, // CDMX
                    ToLocation = 3, // Monterrey
                    DepartureTime = baseDate.AddHours(10),
                    ArrivalTime = baseDate.AddHours(20),
                    ScheduleDate = baseDate,
                    Price = 650.00m,
                    TotalSeats = 38
                },
                new BusSchedule
                {
                    ScheduleId = 3,
                    VendorId = 2,
                    BusName = "Primera Plus GDL-CUN",
                    BusVehicleNo = "PP-101",
                    FromLocation = 2, // Guadalajara
                    ToLocation = 5, // Cancún
                    DepartureTime = baseDate.AddHours(22),
                    ArrivalTime = baseDate.AddDays(1).AddHours(16),
                    ScheduleDate = baseDate,
                    Price = 980.00m,
                    TotalSeats = 40
                }
            );
        }
    }
}
