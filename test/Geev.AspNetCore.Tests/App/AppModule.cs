using System.Reflection;
using Geev.AspNetCore.App.MultiTenancy;
using Geev.AspNetCore.TestBase;
using Geev.Configuration.Startup;
using Geev.Modules;
using Geev.AspNetCore.Configuration;
using Geev.AspNetCore.Mocks;
using Geev.Auditing;
using Geev.FluentValidation;
using Geev.Localization;
using Geev.MultiTenancy;
using Geev.Reflection.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Geev.AspNetCore.App
{
    [DependsOn(typeof(GeevAspNetCoreTestBaseModule), typeof(GeevFluentValidationModule))]
    public class AppModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            Configuration.ReplaceService<IAuditingStore, MockAuditingStore>();
            Configuration.ReplaceService<ITenantStore, TestTenantStore>();

            Configuration
                .Modules.GeevAspNetCore()
                .CreateControllersForAppServices(
                    typeof(AppModule).GetAssembly()
                );

            Configuration.IocManager.Resolve<IGeevAspNetCoreConfiguration>().RouteConfiguration.Add(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AppModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            var localizationConfiguration = IocManager.IocContainer.Resolve<ILocalizationConfiguration>();
            localizationConfiguration.Languages.Add(new LanguageInfo("en-US", "English", isDefault: true));
            localizationConfiguration.Languages.Add(new LanguageInfo("it", "Italian"));
        }
    }
}
