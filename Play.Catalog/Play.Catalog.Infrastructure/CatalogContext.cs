using MongoDB.Driver;
using Play.Catalog.Application.Interfaces;
using Play.Catalog.Domain.Entities;

namespace Play.Catalog.Infrastructure
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IMongoDbConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            var database = client.GetDatabase(config.DatabaseName);

            Items = database.GetCollection<Item>(config.CollectionName);
        }

        public IMongoCollection<Item> Items { get; }
        public FilterDefinitionBuilder<Item> FilterBuilder { get; } = Builders<Item>.Filter;
    }
}