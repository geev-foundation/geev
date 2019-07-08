using Geev.Configuration.Startup;

namespace Geev.WebApi.OData.Configuration
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure Geev.Web.Api.OData module.
    /// </summary>
    public static class GeevWebApiODataConfigurationExtensions
    {
        /// <summary>
        /// Used to configure Geev.Web.Api.OData module.
        /// </summary>
        public static IGeevWebApiODataModuleConfiguration GeevWebApiOData(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevWebApiODataModuleConfiguration>();
        }
    }
}