using Microsoft.Extensions.Configuration;

namespace Play.Common.MongoDb.Settings
{
    public class MongoDbConfig : IMongoDbConfig
    {
        public MongoDbConfig(IConfiguration configuration, string collectionName)
        {
            string host = configuration.GetValue<string>("MongoDbSettings:Host");
            string port = configuration.GetValue<string>("MongoDbSettings:Port");
            ConnectionString = $"mongodb://{host}:{port}";

            DatabaseName = configuration.GetValue<string>("MongoDbSettings:DatabaseName");
            CollectionName = collectionName;
        }

        public string ConnectionString { get; init; }
        public string DatabaseName { get; init; }
        public string CollectionName { get; init; }
    }
}