using Geev.Configuration.Startup;

namespace Geev.Zero.AspNetCore
{
    /// <summary>
    /// Extension methods for module zero configurations.
    /// </summary>
    public static class ModuleZeroAspNetCoreConfigurationExtensions
    {
        /// <summary>
        /// Configures ABP Zero AspNetCore module.
        /// </summary>
        /// <returns></returns>
        public static IGeevZeroAspNetCoreConfiguration ZeroAspNetCore(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.GeevConfiguration.Get<IGeevZeroAspNetCoreConfiguration>();
        }
    }
}