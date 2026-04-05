namespace billing.Controllers.Requests;

public class CreateInvoiceRequest
{
    public string CustumerName { get; set; } = string.Empty;
    public List<CreateInvoiceItemRequest> Items { get; set; } = new();
}

public class CreateInvoiceItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}