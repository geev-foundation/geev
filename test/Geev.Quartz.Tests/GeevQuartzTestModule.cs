using System.Reflection;
using Geev.Modules;

namespace Geev.Quartz.Tests
{
    [DependsOn(typeof(GeevQuartzModule))]
    public class GeevQuartzTestModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.BackgroundJobs.IsJobExecutionEnabled = true;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
