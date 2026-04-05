using Microsoft.AspNetCore.Mvc;
using inventory.Data;
using Microsoft.EntityFrameworkCore;
using inventory.Entities;


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
    public async Task<IActionResult> CreateInvoice([FromBody] List<DecreaseRequest> requests)
{
    using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        // 1. Agrupa produtos repetidos somando as quantidades
        var grouped = requests
            .GroupBy(r => r.ProductId)
            .Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(x => x.Quantity) })
            .ToList();

        // 2. Decrementa estoque de todos os produtos
        foreach (var item in grouped)
        {
            var affected = await _context.Database.ExecuteSqlRawAsync(
                @"UPDATE ""Products""
                  SET ""Quantity"" = ""Quantity"" - {0}
                  WHERE ""Id"" = {1} AND ""Quantity"" >= {0}",
                item.TotalQuantity,
                item.ProductId
            );

            if (affected == 0)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Insufficient stock for product {item.ProductId}");
            }
        }

        await transaction.CommitAsync();

        // 3. Cria a nota com todos os itens da lista

        return Ok(new
        {
            Message = "Stock decreased successfully for all products."
        });
    }
    catch
    {
        await transaction.RollbackAsync();
        return BadRequest("Não foi possível criar a nota devido a estoque insuficiente.");
    }
}
}



public class DecreaseRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}