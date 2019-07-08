using Geev.Web.Configuration;

namespace Geev.Configuration.Startup
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure ABP Web module.
    /// </summary>
    public static class GeevWebConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP Web module.
        /// </summary>
        public static IGeevWebModuleConfiguration GeevWeb(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevWebModuleConfiguration>();
        }
    }
}