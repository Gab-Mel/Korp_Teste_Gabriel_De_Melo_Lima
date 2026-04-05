namespace inventory.Entities;

public class Product
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string Description { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string Unit { get; set; } = string.Empty;
}