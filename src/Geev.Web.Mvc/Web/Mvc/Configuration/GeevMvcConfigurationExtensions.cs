using Geev.Configuration.Startup;

namespace Geev.Web.Mvc.Configuration
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure Geev.Web.Api module.
    /// </summary>
    public static class GeevMvcConfigurationExtensions
    {
        /// <summary>
        /// Used to configure Geev.Web.Api module.
        /// </summary>
        public static IGeevMvcConfiguration GeevMvc(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevMvcConfiguration>();
        }
    }
}