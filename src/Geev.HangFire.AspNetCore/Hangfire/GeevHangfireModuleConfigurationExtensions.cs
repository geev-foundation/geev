using Geev.BackgroundJobs;
using Geev.Configuration.Startup;

namespace Geev.Hangfire.Configuration
{
    public static class GeevHangfireConfigurationExtensions
    {
        /// <summary>
        /// Configures to use Hangfire for background job management.
        /// </summary>
        public static void UseHangfire(this IBackgroundJobConfiguration backgroundJobConfiguration)
        {
            backgroundJobConfiguration.GeevConfiguration.ReplaceService<IBackgroundJobManager, HangfireBackgroundJobManager>();
        }
    }
}