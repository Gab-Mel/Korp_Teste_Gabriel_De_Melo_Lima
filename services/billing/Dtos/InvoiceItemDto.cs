namespace BillingService.DTOs;

public class InvoiceItemDto
{
    public required string Description { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

    public decimal Total { get; set; }
}