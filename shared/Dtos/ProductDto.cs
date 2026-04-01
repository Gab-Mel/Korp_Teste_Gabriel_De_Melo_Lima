namespace Shared.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
}