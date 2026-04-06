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
        string customerName,
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
            var product = await _inventoryService.GetProduct(item.productId);

            if (product == null)
                throw new Exception($"Produto {item.productId} não encontrado");

            invoiceItems.Add(new InvoiceItem
            {
                ProductId = item.productId,
                UnitPrice = product.Price,
                Description = product.Name,
                Quantity = item.quantity,
                Total = product.Price * item.quantity
            });
        }

          
        var invoice = new Invoice
        {   
            CustomerName = customerName,
            IdempotencyKey = idempotencyKey,
            Items = invoiceItems
        };

        invoice.Total = invoiceItems.Sum(i => i.Total);

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