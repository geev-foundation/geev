using System.Reflection;
using Geev.Modules;
using Geev.Zero.Configuration;
using Geev.Zero.SampleApp.Authorization;
using Geev.Zero.SampleApp.Configuration;
using Geev.Zero.SampleApp.Features;

namespace Geev.Zero.SampleApp
{
    [DependsOn(typeof(GeevZeroCoreModule))]
    public class SampleAppModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Features.Providers.Add<AppFeatureProvider>();

            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();
            Configuration.Settings.Providers.Add<AppSettingProvider>();
            Configuration.MultiTenancy.IsEnabled = true;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
