using Microsoft.EntityFrameworkCore;
using inventory.Entities;

namespace inventory.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();

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
    });

    // INVOICE
    modelBuilder.Entity<Invoice>(entity =>
    {
        entity.HasKey(i => i.Id);

        entity.Property(i => i.Status)
              .IsRequired();

        entity.HasIndex(i => i.Number)
              .IsUnique();
    });

    // INVOICE ITEM
    modelBuilder.Entity<InvoiceItem>(entity =>
    {
        entity.HasKey(ii => ii.Id);

        entity.Property(ii => ii.Quantity)
              .IsRequired();

        entity.HasOne(ii => ii.Invoice)
              .WithMany(i => i.Items)
              .HasForeignKey(ii => ii.InvoiceId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(ii => ii.Product)
              .WithMany()
              .HasForeignKey(ii => ii.ProductId);

        entity.HasIndex(ii => new { ii.InvoiceId, ii.ProductId })
              .IsUnique();
    });
}
}