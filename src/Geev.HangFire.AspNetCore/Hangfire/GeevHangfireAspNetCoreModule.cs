using Geev.Modules;
using Geev.Reflection.Extensions;

namespace Geev.Hangfire
{
    [DependsOn(typeof(GeevKernelModule))]
    public class GeevHangfireAspNetCoreModule : GeevModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevHangfireAspNetCoreModule).GetAssembly());
        }
    }
}
