using Play.Inventory.Service.Contracts.v1;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Play.Inventory.Service.Clients
{
    public class CatalogClient
    {
        private readonly HttpClient _httpClient;

        public CatalogClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<CatalogItemDto>> GetCatalogItemsAsync()
        {
            var items = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>("/api/v1/items");

            return items;
        }

        public async Task<CatalogItemDto> GetCatalogItemAsync(Guid id)
        {
            var item = await _httpClient.GetFromJsonAsync<CatalogItemDto>($"/api/v1/items/{id}");

            return item;
        }
    }
}