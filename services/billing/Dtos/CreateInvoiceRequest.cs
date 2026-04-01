using Shared.Dtos;
using System.Collections.Generic;

namespace Billing.Dtos
{
    public class CreateInvoiceRequest
    {
        public List<InvoiceItemDto> Items { get; set; } = new();
    }
}