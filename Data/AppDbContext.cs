using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fixly.Models;

namespace Fixly.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ServiceProviderProfile> ServiceProviderProfiles { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<WorkImage> WorkImages { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Customer)
                .WithMany()
                .HasForeignKey(sr => sr.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Provider)
                .WithMany()
                .HasForeignKey(sr => sr.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}