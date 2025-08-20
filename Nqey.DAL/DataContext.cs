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
            modelBuilder.Entity<User>().UseTptMappingStrategy();

            base.OnModelCreating(modelBuilder);

            var passwordHasher = new PasswordHasher<Provider>();

            modelBuilder.Entity<User>()
                .HasKey(u=> u.UserId);
            

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Provider>().ToTable("Providers");


            modelBuilder.Entity<Location>().OwnsOne(l=> l.Position);
           
            modelBuilder.Entity<Provider>()
                .HasMany(p => p.Portfolio)
                .WithOne(i => i.Provider)
                .HasForeignKey(i => i.ProviderUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.ProfileImage)
                .WithOne(p => p.User)
                .HasForeignKey<ProfileImage>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Service>()
                .HasOne(s => s.ServiceImage)
                .WithOne()
                .HasForeignKey<Service>(s => s.ServiceImageId)
                .IsRequired(false);

            modelBuilder.Entity<SubService>()
                .HasOne(s => s.Provider)
                .WithMany(p => p.SubServices)
                .HasForeignKey(s => s.ProviderUserId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Reservation>()
                 .HasMany(r => r.Events)
                 .WithOne(e => e.Reservation)
                 .HasForeignKey(r => r.ReservationId)
                 .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Client)
                .WithMany()
                .HasForeignKey(r => r.ClientUserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Provider)
                .WithMany()
                .HasForeignKey(r => r.ProviderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReservationEvent>()
                .Property(e => e.ReservationEventType)
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
            

            modelBuilder.Entity<JobDescription>()
                .HasOne(j => j.Reservation)
                .WithOne(r => r.JobDescription)
                .HasForeignKey<JobDescription>(j => j.ReservationId);

            modelBuilder.Entity<Provider>()
                .HasOne(p => p.SelfieImage)
                .WithOne()
                .HasForeignKey<Provider>(p=> p.SelfieId)
                .IsRequired(false);

            modelBuilder.Entity<Provider>()
                .HasOne(p => p.IdentityPiece)
                .WithOne()
                .HasForeignKey<Provider>(p => p.IdentityId)
                .IsRequired(false);
            
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Client)
                .WithMany() // one-way: client has no collection
                .HasForeignKey(r => r.ClientUserId)
                .OnDelete(DeleteBehavior.Restrict); // prevent cascade

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Provider)
                .WithMany(p => p.Reviews) // provider has the collection
                .HasForeignKey(r => r.ProviderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<User>()
            //    .ToTable("Users");
            //modelBuilder.Entity<Provider>()
            //    .ToTable("Providers");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationEvent> ReservationEvents { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<JobDescription> JobDescriptions  { get; set; }
        public DbSet<Admin> Admins { get; set; }

    }
}
