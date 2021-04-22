using Play.Catalog.Application.Interfaces;

namespace Play.Catalog.Api.Helpers
{
    public class MongoDbConfig : IMongoDbConfig
    {
        public MongoDbConfig()
        {
            ConnectionString = "mongodb://localhost:27017";
            DatabaseName = "Catalog";
            CollectionName = "items";
        }
        
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}