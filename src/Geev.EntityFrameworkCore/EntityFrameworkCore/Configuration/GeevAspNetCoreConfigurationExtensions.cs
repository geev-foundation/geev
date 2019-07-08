using Geev.Configuration.Startup;

namespace Geev.EntityFrameworkCore.Configuration
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure ABP EntityFramework Core module.
    /// </summary>
    public static class GeevEfCoreConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP EntityFramework Core module.
        /// </summary>
        public static IGeevEfCoreConfiguration GeevEfCore(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevEfCoreConfiguration>();
        }
    }
}