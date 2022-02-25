using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PooLandApp.Data
{
    public partial class PooLandDbContext : DbContext
    {
        public PooLandDbContext()
        {
        }

        public PooLandDbContext(DbContextOptions<PooLandDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Poodatum> Poodata { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=poolanddb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Poodatum>(entity =>
            {
                entity.ToTable("poodata");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date).HasColumnName("date").HasDefaultValue(DateTime.UtcNow);

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.Latitude).HasColumnName("latitude");

                entity.Property(e => e.Longitude).HasColumnName("longitude");

                entity.Property(e => e.Photo).HasColumnName("photo");

                entity.Property(e => e.Visible).HasColumnName("visible");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
