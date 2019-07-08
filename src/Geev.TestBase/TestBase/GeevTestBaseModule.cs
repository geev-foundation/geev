using Geev.Modules;
using Geev.Reflection.Extensions;

namespace Geev.TestBase
{
    [DependsOn(typeof(GeevKernelModule))]
    public class GeevTestBaseModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.EventBus.UseDefaultEventBus = false;
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevTestBaseModule).GetAssembly());
        }
    }
}