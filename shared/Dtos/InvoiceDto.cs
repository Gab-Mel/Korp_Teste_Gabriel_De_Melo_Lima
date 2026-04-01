using System;
using System.Collections.Generic;

namespace Shared.Dtos;

public class InvoiceDto
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public bool IsClosed { get; set; }
    public DateTime CreatedAt { get; set; }

    // Itens da nota
    public List<InvoiceItemDto> Items { get; set; } = new();
}