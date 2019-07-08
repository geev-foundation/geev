using Geev.Configuration.Startup;

namespace Geev.BackgroundJobs
{
    internal class BackgroundJobConfiguration : IBackgroundJobConfiguration
    {
        public bool IsJobExecutionEnabled { get; set; }
        
        public IGeevStartupConfiguration GeevConfiguration { get; private set; }

        public BackgroundJobConfiguration(IGeevStartupConfiguration geevConfiguration)
        {
            GeevConfiguration = geevConfiguration;

            IsJobExecutionEnabled = true;
        }
    }
}