public class UpdateInvoiceRequest
{
    public List<InvoiceItemRequest> Items { get; set; } = new List<InvoiceItemRequest>();
}

public class InvoiceItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}