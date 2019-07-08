using Geev.WebApi.Configuration;

namespace Geev.Configuration.Startup
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure Geev.Web.Api module.
    /// </summary>
    public static class GeevWebApiConfigurationExtensions
    {
        /// <summary>
        /// Used to configure Geev.Web.Api module.
        /// </summary>
        public static IGeevWebApiConfiguration GeevWebApi(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevWebApiConfiguration>();
        }
    }
}