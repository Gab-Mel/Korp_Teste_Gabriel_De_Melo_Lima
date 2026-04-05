using System.Net.Http.Json;

public class InventoryService
{
    private readonly HttpClient _http;

    public InventoryService(HttpClient http)
    {
        _http = http;
    }

    public class DecreaseRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public async Task<bool> DecreaseStock(List<DecreaseRequest> requests)
    {
        var response = await _http.PostAsJsonAsync(
            "/api/inventory/decrease",
            requests
        );

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Erro Inventory: {error}");
        }

        return response.IsSuccessStatusCode;
    }
}