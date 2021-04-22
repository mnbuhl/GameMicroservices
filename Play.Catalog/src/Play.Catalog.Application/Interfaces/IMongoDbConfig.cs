namespace Play.Catalog.Application.Interfaces
{
    public interface IMongoDbConfig
    {
        string ConnectionString { get; init; }
        string DatabaseName { get; init; }
        string CollectionName { get; init; }
    }
}