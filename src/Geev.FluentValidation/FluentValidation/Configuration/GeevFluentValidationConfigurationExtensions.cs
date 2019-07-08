using Geev.Configuration.Startup;

namespace Geev.FluentValidation.Configuration
{
    public static class GeevFluentValidationConfigurationExtensions
    {
        /// <summary>
        /// Used to configure Geev.FluentValidation module.
        /// </summary>
        public static IGeevFluentValidationConfiguration GeevFluentValidation(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevFluentValidationConfiguration>();
        }
    }
}
