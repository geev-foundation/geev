using Geev.MemoryDb.Configuration;

namespace Geev.Configuration.Startup
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure ABP MemoryDb module.
    /// </summary>
    public static class GeevMemoryDbConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP MemoryDb module.
        /// </summary>
        public static IGeevMemoryDbModuleConfiguration GeevMemoryDb(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevMemoryDbModuleConfiguration>();
        }
    }
}