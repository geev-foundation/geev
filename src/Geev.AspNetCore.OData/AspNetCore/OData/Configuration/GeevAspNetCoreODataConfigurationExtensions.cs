using Geev.Configuration.Startup;

namespace Geev.AspNetCore.OData.Configuration
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure Geev.AspNetCore.OData module.
    /// </summary>
    public static class GeevAspNetCoreODataConfigurationExtensions
    {
        /// <summary>
        /// Used to configure Geev.AspNetCore.OData module.
        /// </summary>
        public static IGeevAspNetCoreODataModuleConfiguration GeevAspNetCoreOData(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevAspNetCoreODataModuleConfiguration>();
        }
    }
}
