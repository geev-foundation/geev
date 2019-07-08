using Quartz;
using Quartz.Impl;

namespace Geev.Quartz.Configuration
{
    public class GeevQuartzConfiguration : IGeevQuartzConfiguration
    {
        public IScheduler Scheduler => StdSchedulerFactory.GetDefaultScheduler().Result;
    }
}