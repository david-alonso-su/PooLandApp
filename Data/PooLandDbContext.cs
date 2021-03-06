using Microsoft.EntityFrameworkCore;

namespace PooLandApp.Data;
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
    public virtual DbSet<Neighborhood> Neighborhoods { get; set; } = null!;

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

            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

            entity.Property(e => e.Date).HasColumnName("date").HasDefaultValue(DateTime.UtcNow);

            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");

            entity.Property(e => e.Photo).HasColumnName("photo");

            entity.Property(e => e.Visible).HasColumnName("visible");

            entity.Property(e => e.Location);

            entity.Property(e => e.NeighborhoodId);

            entity.HasKey(e => e.Id);

            entity.HasOne<Neighborhood>(s => s.Neighborhood).WithMany(g => g.Poodatums).HasForeignKey(s => s.NeighborhoodId);
        });

        modelBuilder.Entity<Neighborhood>(entity =>
        {
            entity.ToTable("neighborhood");
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasColumnName("description");
            entity.Property(e => e.Coordinates);

            entity.HasKey(e => e.Id);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
