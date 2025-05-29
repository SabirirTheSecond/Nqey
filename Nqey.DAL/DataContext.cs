using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nqey.Domain;
using Nqey.Domain.Common;

namespace Nqey.DAL
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
        
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var passwordHasher = new PasswordHasher<Provider>();

            modelBuilder.Entity<Location>().OwnsOne(l=> l.Position);
           
            modelBuilder.Entity<Provider>()
                .HasMany(p => p.Portfolio)
                .WithOne(i => i.Provider)
                .HasForeignKey(i => i.ProviderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.ProfileImage)
                .WithOne(p => p.User)
                .HasForeignKey<ProfileImage>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Provider>()
                .HasOne(p => p.ProfileImage)
                .WithOne()
                .HasForeignKey<Provider>(p => p.PImageId)
                .IsRequired(false);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.ProfileImage)
                .WithOne()
                .HasForeignKey<Client>(c => c.PImageId)
                .IsRequired(false);

            modelBuilder.Entity<Service>()
                .HasOne(s => s.ServiceImage)
                .WithOne()
                .HasForeignKey<Service>(s => s.ServiceImageId)
                .IsRequired(false);

            modelBuilder.Entity<SubService>()
                .HasOne(s => s.Provider)
                .WithMany(p => p.SubServices)
                .HasForeignKey(s => s.ProviderId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Reservation>()
                 .HasMany(r => r.Events)
                 .WithOne(e => e.Reservation)
                 .HasForeignKey(r => r.ReservationId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReservationEvent>()
                .Property(e => e.ReservationEventType)
                .HasConversion<string>();

            modelBuilder.Entity<Provider>()
                .Property(p=>p.AccountStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Client>()
                .Property(c => c.Status)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.AccountStatus)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.UserRole)
                .HasConversion<string>();

            modelBuilder.Entity<Reservation>()
                .Property(r => r.Status)
                .HasConversion<string>();
            modelBuilder.Entity<Provider>()
                .Property(u => u.UserRole)
                .HasConversion<string>();
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationEvent> ReservationEvents { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<JobDescription> JobDescriptions  { get; set; }

    }
}
