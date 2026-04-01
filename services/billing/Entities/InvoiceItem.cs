namespace billing.Entities;

public class InvoiceItem
{
    public int Id { get; set; }

    public int InvoiceId { get; set; }

    public Invoice Invoice { get; set; } = null!;

    public int ProductId { get; set; }

    public int Quantity { get; set; }
}