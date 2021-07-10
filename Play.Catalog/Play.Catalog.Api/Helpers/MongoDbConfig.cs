using Microsoft.Extensions.Configuration;
using Play.Catalog.Application.Interfaces;

namespace Play.Catalog.Api.Helpers
{
    public class MongoDbConfig : IMongoDbConfig
    {
        public MongoDbConfig(IConfiguration configuration)
        {
            string host = configuration.GetValue<string>("MongoDbSettings:Host");
            string port = configuration.GetValue<string>("MongoDbSettings:Port");
            ConnectionString = $"mongodb://{host}:{port}";
            
            DatabaseName = configuration.GetValue<string>("MongoDbSettings:DatabaseName");
            CollectionName = "items";
        }
        
        public string ConnectionString { get; init; }
        public string DatabaseName { get; init; }
        public string CollectionName { get; init; }
    }
}