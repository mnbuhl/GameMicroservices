namespace Play.Common.Settings
{
    public interface IMongoDbConfig
    {
        string ConnectionString { get; init; }
        string DatabaseName { get; init; }
        string CollectionName { get; init; }
    }
}