using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using billing.Data;
using billing.Entities;
using billing.Dtos;

namespace billing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly AppDbContext _context;

    public InvoiceController(AppDbContext context)
    {
        _context = context;
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
    public async Task<ActionResult<Invoice>> Create(CreateInvoiceRequest request)
    {
        var invoice = new Invoice
        {
            Items = request.Items.Select(i => new InvoiceItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
    }

    // PUT: api/invoice/1 (fechar invoice)
    [HttpPut("{id}/close")]
    public async Task<IActionResult> Close(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);

        if (invoice == null)
            return NotFound();

        invoice.Status = "Closed";

        await _context.SaveChangesAsync();

        return NoContent();
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