using System.Net.Http.Json;
using Northwind.Shared.Products;

namespace Northwind.Client.Services;

public class ProductApiClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IReadOnlyCollection<ProductDto>> GetProductsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<ProductDto>>("api/products");
        return result ?? [];
    }

    public async Task<ProductDto?> GetProductAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/products/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<ProductDto>();
    }

    public async Task<ProductDto?> CreateProductAsync(UpsertProductDto dto) =>
        await SendAsync<ProductDto>(() => _httpClient.PostAsJsonAsync("api/products", dto));

    public async Task<ProductDto?> UpdateProductAsync(int id, UpsertProductDto dto) =>
        await SendAsync<ProductDto>(() => _httpClient.PutAsJsonAsync($"api/products/{id}", dto));

    public async Task<bool> DeleteProductAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/products/{id}");
        return response.IsSuccessStatusCode;
    }

    private static async Task<T?> SendAsync<T>(Func<Task<HttpResponseMessage>> factory)
    {
        var response = await factory();
        if (!response.IsSuccessStatusCode)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<T>();
    }
}

