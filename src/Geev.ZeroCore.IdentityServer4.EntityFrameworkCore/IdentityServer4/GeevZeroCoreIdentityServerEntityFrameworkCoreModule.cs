using Geev.Modules;
using Geev.Reflection.Extensions;
using Geev.Zero.EntityFrameworkCore;

namespace Geev.IdentityServer4
{
    [DependsOn(typeof(GeevZeroCoreIdentityServerModule), typeof(GeevZeroCoreEntityFrameworkCoreModule))]
    public class GeevZeroCoreIdentityServerEntityFrameworkCoreModule : GeevModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevZeroCoreIdentityServerEntityFrameworkCoreModule).GetAssembly());
        }
    }
}
