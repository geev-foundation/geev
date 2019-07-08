using Geev.Modules;
using Geev.Reflection.Extensions;

namespace Geev.EntityFramework
{
    [DependsOn(typeof(GeevKernelModule))]
    public class GeevEntityFrameworkCommonModule : GeevModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevEntityFrameworkCommonModule).GetAssembly());
        }
    }
}
