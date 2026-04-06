using Humanizer;

namespace billing.Entities;

public class Invoice
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Status { get; set; } = "OPEN";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal Total { get; set; }
    public List<InvoiceItem> Items { get; set; } = new();
    public string IdempotencyKey { get; set; } = string.Empty;
}