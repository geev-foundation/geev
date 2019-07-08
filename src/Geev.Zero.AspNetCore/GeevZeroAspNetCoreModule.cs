using System.Reflection;
using Geev.Authorization.Users;
using Geev.Configuration.Startup;
using Geev.Modules;

namespace Geev.Zero.AspNetCore
{
    [DependsOn(typeof(GeevZeroCoreModule))]
    public class GeevZeroAspNetCoreModule : GeevModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IGeevZeroAspNetCoreConfiguration, GeevZeroAspNetCoreConfiguration>();
            Configuration.ReplaceService<IUserTokenProviderAccessor, AspNetCoreUserTokenProviderAccessor>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}