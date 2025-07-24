using InTime.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InTime.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<FirebaseDevice> FirebaseDevices => Set<FirebaseDevice>();
        public DbSet<GoogleCalendarIntegration> CalendarIntegrations => Set<GoogleCalendarIntegration>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Tenant has many Users
            builder.Entity<Tenant>()
                .HasMany(t => t.Users)
                .WithOne(u => u.Tenant)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // ApplicationUser has many Attendances
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Attendances)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            // ApplicationUser has many Firebase Devices
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Devices)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId);

            // ApplicationUser has optional Calendar Integration
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.CalendarIntegration)
                .WithOne(ci => ci.User)
                .HasForeignKey<GoogleCalendarIntegration>(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
