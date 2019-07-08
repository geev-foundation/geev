using Geev.Dependency;
using Geev.Extensions;
using Quartz;
using Quartz.Spi;

namespace Geev.Quartz
{
    public class GeevQuartzJobFactory : IJobFactory
    {
        private readonly IIocResolver _iocResolver;

        public GeevQuartzJobFactory(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _iocResolver.Resolve(bundle.JobDetail.JobType).As<IJob>();
        }

        public void ReturnJob(IJob job)
        {
            _iocResolver.Release(job);
        }
    }
}