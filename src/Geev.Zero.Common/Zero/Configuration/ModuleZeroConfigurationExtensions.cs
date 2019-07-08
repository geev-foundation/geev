using Geev.Configuration.Startup;

namespace Geev.Zero.Configuration
{
    /// <summary>
    /// Extension methods for module zero configurations.
    /// </summary>
    public static class ModuleZeroConfigurationExtensions
    {
        /// <summary>
        /// Used to configure module zero.
        /// </summary>
        /// <param name="moduleConfigurations"></param>
        /// <returns></returns>
        public static IGeevZeroConfig Zero(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.GeevConfiguration.Get<IGeevZeroConfig>();
        }
    }
}