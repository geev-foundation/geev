using Geev.Modules;
using Geev.Reflection.Extensions;
using Geev.TestBase;

namespace Geev.AspNetCore.TestBase
{
    [DependsOn(typeof(GeevTestBaseModule),typeof(GeevAspNetCoreModule))]
    public class GeevAspNetCoreTestBaseModule : GeevModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevAspNetCoreTestBaseModule).GetAssembly());
        }
    }
}