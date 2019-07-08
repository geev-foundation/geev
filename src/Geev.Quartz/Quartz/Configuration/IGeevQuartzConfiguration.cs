using Quartz;

namespace Geev.Quartz.Configuration
{
    public interface IGeevQuartzConfiguration
    {
        IScheduler Scheduler { get;}
    }
}