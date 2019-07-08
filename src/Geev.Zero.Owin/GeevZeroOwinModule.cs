using System.Reflection;
using Geev.Authorization.Users;
using Geev.Modules;
using Geev.Zero;
using Geev.Configuration.Startup;
using Geev.Owin;

namespace Geev
{
    [DependsOn(typeof(GeevZeroCoreModule))]
    public class GeevZeroOwinModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.ReplaceService<IUserTokenProviderAccessor, OwinUserTokenProviderAccessor>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
