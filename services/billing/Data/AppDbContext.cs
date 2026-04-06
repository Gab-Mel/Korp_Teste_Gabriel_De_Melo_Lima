using Microsoft.EntityFrameworkCore;
using billing.Entities;

namespace billing.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // INVOICE
    modelBuilder.Entity<Invoice>(entity =>
    {
        entity.HasKey(i => i.Id);

        entity.Property(i => i.Id)
              .ValueGeneratedOnAdd();

        entity.Property(i => i.CustomerName)
              .IsRequired();

        entity.Property(i => i.Status)
              .IsRequired();

        entity.HasIndex(i => i.Number)
              .IsUnique();
              
        entity.HasIndex(i => i.IdempotencyKey)
              .IsUnique();
    });

    // INVOICE ITEM
    modelBuilder.Entity<InvoiceItem>(entity =>
    {
        entity.HasKey(ii => ii.Id);
        entity.Property(ii => ii.Id)
              .ValueGeneratedOnAdd();

        entity.Property(ii => ii.Quantity)
              .IsRequired();

        entity.HasOne(ii => ii.Invoice)
              .WithMany(i => i.Items)
              .HasForeignKey(ii => ii.InvoiceId)
              .OnDelete(DeleteBehavior.Cascade);

        // entity.HasOne(ii => ii.Product)
        //       .WithMany()
        //       .HasForeignKey(ii => ii.ProductId);

        entity.HasIndex(ii => new { ii.InvoiceId, ii.ProductId })
              .IsUnique();
    });
}
}