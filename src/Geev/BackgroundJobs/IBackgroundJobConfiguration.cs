using Geev.Configuration.Startup;

namespace Geev.BackgroundJobs
{
    /// <summary>
    /// Used to configure background job system.
    /// </summary>
    public interface IBackgroundJobConfiguration
    {
        /// <summary>
        /// Used to enable/disable background job execution.
        /// </summary>
        bool IsJobExecutionEnabled { get; set; }

        /// <summary>
        /// Gets the ABP configuration object.
        /// </summary>
        IGeevStartupConfiguration GeevConfiguration { get; }
    }
}