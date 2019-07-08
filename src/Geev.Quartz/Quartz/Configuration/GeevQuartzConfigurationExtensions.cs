using Geev.Configuration.Startup;

namespace Geev.Quartz.Configuration
{
    public static class GeevQuartzConfigurationExtensions
    {
        /// <summary>
        ///     Used to configure ABP Quartz module.
        /// </summary>
        public static IGeevQuartzConfiguration GeevQuartz(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevQuartzConfiguration>();
        }
    }
}
