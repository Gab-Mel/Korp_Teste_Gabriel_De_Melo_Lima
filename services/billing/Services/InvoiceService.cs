using billing.Data;
using billing.Entities;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class InvoiceService
{
    private readonly AppDbContext _context;
    private readonly InventoryService _inventoryService;

    public InvoiceService(AppDbContext context, InventoryService inventoryService)
    {
        _context = context;
        _inventoryService = inventoryService;
    }

    public async Task<Invoice> CreateInvoice(
        string custumerName,
        List<(int productId, int quantity)> items,
        string idempotencyKey)
    {
        // 🔥 1. IDMPOTÊNCIA → verifica se já existe
        var existing = await _context.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.IdempotencyKey == idempotencyKey);

        if (existing != null)
            return existing;

        var invoiceItems = new List<InvoiceItem>();

        foreach (var item in items)
        {
            // var success = await _inventoryService.DecreaseStock(item.productId, item.quantity);

            // if (!success)
            //     throw new Exception($"Erro ao baixar estoque do produto {item.productId}");

            invoiceItems.Add(new InvoiceItem
            {
                ProductId = item.productId,
                Quantity = item.quantity
            });
        }

        // 🔥 3. cria invoice SEM confiar no Max (ainda vamos proteger)
        var invoice = new Invoice
        {   
            CustumerName = custumerName,
            IdempotencyKey = idempotencyKey,
            Items = invoiceItems
        };

        _context.Invoices.Add(invoice);

        try
        {
            // 🔥 4. resolve concorrência do Number aqui
            invoice.Number = (_context.Invoices.Max(i => (int?)i.Number) ?? 0) + 1;

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // 🔥 concorrência → tenta recuperar existente
            var retry = await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.IdempotencyKey == idempotencyKey);

            if (retry != null)
                return retry;

            throw;
        }

        return invoice;
    }
}