using Microsoft.EntityFrameworkCore;
using inventory.Entities;

namespace inventory.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // PRODUCT
    modelBuilder.Entity<Product>(entity =>
    {
        entity.HasKey(p => p.Id);

        entity.Property(p => p.Description)
              .IsRequired();

        entity.Property(p => p.Quantity)
              .IsRequired();

        entity.ToTable(t => t.HasCheckConstraint("CK_Product_Quantity", "\"Quantity\" >= 0"));

        entity.HasIndex(p => p.Code)
              .IsUnique();
        
        entity.Property(p => p.Price)
              .IsRequired();

        entity.Property(p => p.Unit)
              .IsRequired();
    });

}
}