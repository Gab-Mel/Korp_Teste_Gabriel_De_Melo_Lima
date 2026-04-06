
using System;
using System.Collections.Generic;
using System.Linq;
using billing.Entities;
namespace BillingService.DTOs;

public class InvoiceDto
{
    public int Id { get; set; }
    public required string CustomerName { get; set; }
    public DateTime Date { get; set; }

    public List<InvoiceItemDto> Items { get; set; } = new();

    public decimal Total { get; set; }
}