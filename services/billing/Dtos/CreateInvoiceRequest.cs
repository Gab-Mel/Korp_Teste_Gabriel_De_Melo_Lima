using Shared.Dtos;
using System.Collections.Generic;

namespace billing.Dtos
{
    public class CreateInvoiceRequest
    {
        public List<InvoiceItemDto> Items { get; set; } = new();
    }

    public class InvoiceItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
    }