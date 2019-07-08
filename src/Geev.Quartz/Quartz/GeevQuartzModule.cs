using System.Reflection;
using Geev.Dependency;
using Geev.Modules;
using Geev.Quartz.Configuration;
using Geev.Threading.BackgroundWorkers;
using Quartz;

namespace Geev.Quartz
{
    [DependsOn(typeof (GeevKernelModule))]
    public class GeevQuartzModule : GeevModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IGeevQuartzConfiguration, GeevQuartzConfiguration>();

            Configuration.Modules.GeevQuartz().Scheduler.JobFactory = new GeevQuartzJobFactory(IocManager);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.RegisterIfNot<IJobListener, GeevQuartzJobListener>();

            Configuration.Modules.GeevQuartz().Scheduler.ListenerManager.AddJobListener(IocManager.Resolve<IJobListener>());

            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                IocManager.Resolve<IBackgroundWorkerManager>().Add(IocManager.Resolve<IQuartzScheduleJobManager>());
            }
        }
    }
}
