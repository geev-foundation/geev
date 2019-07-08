using System.Reflection;
using Geev.Modules;
using Geev.Reflection.Extensions;

namespace Geev.Runtime.Caching.Redis
{
    /// <summary>
    /// This modules is used to replace ABP's cache system with Redis server.
    /// </summary>
    [DependsOn(typeof(GeevKernelModule))]
    public class GeevRedisCacheModule : GeevModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<GeevRedisCacheOptions>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevRedisCacheModule).GetAssembly());
        }
    }
}
