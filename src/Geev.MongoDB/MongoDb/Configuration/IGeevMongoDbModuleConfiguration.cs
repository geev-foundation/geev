namespace Geev.MongoDb.Configuration
{
    public interface IGeevMongoDbModuleConfiguration
    {
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }
    }
}
