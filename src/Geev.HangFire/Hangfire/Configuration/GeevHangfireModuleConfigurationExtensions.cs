using System;
using Geev.BackgroundJobs;
using Geev.Configuration.Startup;

namespace Geev.Hangfire.Configuration
{
    public static class GeevHangfireConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP Hangfire module.
        /// </summary>
        public static IGeevHangfireConfiguration GeevHangfire(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevHangfireConfiguration>();
        }

        /// <summary>
        /// Configures to use Hangfire for background job management.
        /// </summary>
        public static void UseHangfire(this IBackgroundJobConfiguration backgroundJobConfiguration, Action<IGeevHangfireConfiguration> configureAction)
        {
            backgroundJobConfiguration.GeevConfiguration.ReplaceService<IBackgroundJobManager, HangfireBackgroundJobManager>();
            configureAction(backgroundJobConfiguration.GeevConfiguration.Modules.GeevHangfire());
        }
    }
}