using ASC.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASC.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ServiceRequest> ServiceRequests { get; set; } = null!;
        public DbSet<MasterDataKey> MasterDataKeys { get; set; } = null!;
        public DbSet<MasterDataValue> MasterDataValues { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ServiceRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CustomerId).HasMaxLength(450);
                entity.Property(e => e.CustomerName).HasMaxLength(200);
                entity.Property(e => e.EngineerId).HasMaxLength(450);
                entity.Property(e => e.EngineerName).HasMaxLength(200);
                entity.Property(e => e.ServiceType).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.Notes).HasMaxLength(500);

                // Thêm precision cho decimal
                entity.Property(e => e.EstimatedCost)
                    .HasPrecision(18, 2);
                entity.Property(e => e.ActualCost)
                    .HasPrecision(18, 2);
            });

            builder.Entity<MasterDataKey>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Key).IsUnique();
                entity.Property(e => e.Key).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Category).HasMaxLength(100);
            });

            builder.Entity<MasterDataValue>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Key).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Value).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.DisplayOrder);
            });

            builder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ProductCode).IsUnique();
                entity.Property(e => e.ProductCode).HasMaxLength(50).IsRequired();
                entity.Property(e => e.ProductName).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);

                // Thêm precision cho decimal
                entity.Property(e => e.Price)
                    .HasPrecision(18, 2);
            });
        }
    }
}