using Geev.Modules;
using Geev.Reflection.Extensions;
using Geev.TestBase;

namespace Geev.Web.Common.Tests
{
    [DependsOn(typeof(GeevWebCommonModule), typeof(GeevTestBaseModule))]
    public class GeevWebCommonTestModule : GeevModule
    {
        public override void PreInitialize()
        {
            base.PreInitialize();

            Configuration.Settings.Providers.Add<GeevWebCommonTestModuleSettingProvider>();
            Configuration.Authorization.Providers.Add<GeevWebCommonTestModuleAuthProvider>();
        }

        public override void Initialize()
        {
            base.Initialize();

            IocManager.RegisterAssemblyByConvention(typeof(GeevWebCommonTestModule).GetAssembly());
        }
    }
}
