using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shared.Dtos;

namespace Billing.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDto?> GetProductAsync(int productId)
        {
            return await _httpClient.GetFromJsonAsync<ProductDto>(
                $"http://inventory-service/products/{productId}"
            );
        }

        public async Task ValidateProductExistsAsync(int productId)
        {
            var product = await GetProductAsync(productId);
            if (product == null)
            {
                throw new Exception($"Product {productId} does not exist in Inventory.");
            }
        }
    }
}