using Microsoft.AspNetCore.Mvc;
using inventory.Data;
using Microsoft.EntityFrameworkCore;

namespace inventory.Controllers;

[ApiController]
[Route("api/inventory")]
public class InventoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public InventoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("decrease")]
    public async Task<IActionResult> Decrease([FromBody] DecreaseRequest request)
    {
        var affected = await _context.Database.ExecuteSqlRawAsync(
            @"UPDATE ""Products""
              SET ""Quantity"" = ""Quantity"" - {0}
              WHERE ""Id"" = {1} AND ""Quantity"" >= {0}",
            request.Quantity,
            request.ProductId
        );

        if (affected == 0)
            return BadRequest("Insufficient stock");

        return Ok();
    }
}

public class DecreaseRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}