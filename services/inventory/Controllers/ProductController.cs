using Microsoft.AspNetCore.Mvc;
using inventory.Data;
using inventory.Entities;
using Microsoft.EntityFrameworkCore;

namespace inventory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/product
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> Get()
    {
        return await _context.Products.ToListAsync();
    }

    // GET: api/product/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> Get(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    // POST: api/product
    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    // PUT: api/product/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product updated)
    {
        if (id != updated.Id)
            return BadRequest("ID mismatch");

        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        if (updated.Quantity < 0)
            return BadRequest("Quantity cannot be negative");

        if (string.IsNullOrWhiteSpace(product.Description))
            return BadRequest("Description is required");

        if (updated.Price < 0)
            return BadRequest("Price cannot be negative");

        if (string.IsNullOrWhiteSpace(updated.Description))
            return BadRequest("Description is required");

        if (string.IsNullOrWhiteSpace(updated.Unit))
            return BadRequest("Unit is required");


        // // atualização controlada
        // product.Code = updated.Code;
        // product.Description = updated.Description;
        // product.Quantity = updated.Quantity;
        // product.Price = updated.Price;
        // product.Unit = updated.Unit;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/product/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(int id)
    {
        return _context.Products.Any(p => p.Id == id);
    }
}