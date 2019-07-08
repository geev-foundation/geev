namespace Geev.MongoDb.Configuration
{
    internal class GeevMongoDbModuleConfiguration : IGeevMongoDbModuleConfiguration
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}