using Geev.Hangfire.Configuration;
using Geev.Modules;
using Geev.Reflection.Extensions;
using Hangfire;

namespace Geev.Hangfire
{
    [DependsOn(typeof(GeevKernelModule))]
    public class GeevHangfireModule : GeevModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IGeevHangfireConfiguration, GeevHangfireConfiguration>();
            
            Configuration.Modules
                .GeevHangfire()
                .GlobalConfiguration
                .UseActivator(new HangfireIocJobActivator(IocManager));
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevHangfireModule).GetAssembly());
            GlobalJobFilters.Filters.Add(IocManager.Resolve<GeevHangfireJobExceptionFilter>());
        }
    }
}
