using Geev.Configuration.Startup;

namespace Geev.AspNetCore.Configuration
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure ABP ASP.NET Core module.
    /// </summary>
    public static class GeevAspNetCoreConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP ASP.NET Core module.
        /// </summary>
        public static IGeevAspNetCoreConfiguration GeevAspNetCore(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevAspNetCoreConfiguration>();
        }
    }
}