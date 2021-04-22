using MongoDB.Driver;
using Play.Catalog.Domain.Entities;

namespace Play.Catalog.Application.Interfaces
{
    public interface ICatalogContext
    {
        IMongoCollection<Item> Items { get; }
        FilterDefinitionBuilder<Item> FilterBuilder { get; }
    }
}