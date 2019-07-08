using Geev.MongoDb.Configuration;

namespace Geev.Configuration.Startup
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure ABP MongoDb module.
    /// </summary>
    public static class GeevMongoDbConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP MongoDb module.
        /// </summary>
        public static IGeevMongoDbModuleConfiguration GeevMongoDb(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevMongoDbModuleConfiguration>();
        }
    }
}