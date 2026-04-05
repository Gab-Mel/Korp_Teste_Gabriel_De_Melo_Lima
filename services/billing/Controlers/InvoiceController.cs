using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using billing.Data;
using billing.Entities;
using billing.Controllers.Requests;
using billing.Services;
using Humanizer;

namespace billing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly InvoiceService _service;

    private readonly InventoryService _inventoryService;

    public InvoiceController(AppDbContext context, InvoiceService service, InventoryService inventoryService)
    {
        _context = context;
        _service = service;
        _inventoryService = inventoryService;
    }

    // GET: api/invoice
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Invoice>>> Get()
    {
        return await _context.Invoices
            .Include(i => i.Items)
            .ToListAsync();
    }

    // GET: api/invoice/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Invoice>> GetById(int id)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            return NotFound();

        return invoice;
    }

    // POST: api/invoice
    [HttpPost]
    public async Task<ActionResult<Invoice>> Create(
        [FromBody] CreateInvoiceRequest request,
        [FromHeader(Name = "Idempotency-Key")] string idempotencyKey)
    {
        if (string.IsNullOrEmpty(idempotencyKey))
            return BadRequest("Idempotency-Key header is required.");

        var existingInvoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.IdempotencyKey == idempotencyKey);

        if (existingInvoice != null)
            return Conflict("An invoice with the same Idempotency-Key already exists.");
        

        try
        {
            var items = request.Items
                .Select(i => (i.ProductId, i.Quantity))
                .ToList();

            var invoice = await _service.CreateInvoice(request.CustumerName, items, idempotencyKey);

            return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
        }
        catch (DbUpdateException)
        {
            var existingInvoiceDb = await _context.Invoices
                .FirstOrDefaultAsync(i => i.IdempotencyKey == idempotencyKey);
            if (existingInvoiceDb != null)               
                return Conflict("An invoice with the same Idempotency-Key already exists.");
            throw;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/invoice/1 (fechar invoice)
    [HttpPut("{id}/close")]
    public async Task<IActionResult> Close(int id)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            return NotFound();

        if (invoice.Status == "CLOSED")
            return BadRequest("Invoice is already CLOSED.");

        try
        {   
            var decreaseRequests = invoice.Items
                .Select(item => new InventoryService.DecreaseRequest 
                { 
                    ProductId = item.ProductId, 
                    Quantity = item.Quantity 
                })
                .ToList();

            var success = await _inventoryService.DecreaseStock(decreaseRequests);

            if (!success)
                throw new Exception($"Erro ao baixar estoque da lista de produtos da invoice {id}");

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        invoice.Status = "CLOSED";

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // PUT: api/invoice/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateInvoiceRequest request)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            return NotFound();

        if (invoice.Status != "OPEN")
            return BadRequest("Only open invoices can be updated.");

        // Substituir os itens da invoice
        invoice.Items.Clear();
        foreach (var item in request.Items)
        {
            invoice.Items.Add(new InvoiceItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });
        }

        await _context.SaveChangesAsync();

        return Ok(invoice);
    }

    // DELETE: api/invoice/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);

        if (invoice == null)
            return NotFound();

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}