using System.Linq;
using Geev.AspNetCore.Configuration;
using Geev.AspNetCore.MultiTenancy;
using Geev.AspNetCore.Mvc.Auditing;
using Geev.AspNetCore.Runtime.Session;
using Geev.AspNetCore.Security.AntiForgery;
using Geev.Auditing;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Modules;
using Geev.Reflection.Extensions;
using Geev.Runtime.Session;
using Geev.Web;
using Geev.Web.Security.AntiForgery;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Options;

namespace Geev.AspNetCore
{
    [DependsOn(typeof(GeevWebCommonModule))]
    public class GeevAspNetCoreModule : GeevModule
    {
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new GeevAspNetCoreConventionalRegistrar());

            IocManager.Register<IGeevAspNetCoreConfiguration, GeevAspNetCoreConfiguration>();

            Configuration.ReplaceService<IPrincipalAccessor, AspNetCorePrincipalAccessor>(DependencyLifeStyle.Transient);
            Configuration.ReplaceService<IGeevAntiForgeryManager, GeevAspNetCoreAntiForgeryManager>(DependencyLifeStyle.Transient);
            Configuration.ReplaceService<IClientInfoProvider, HttpContextClientInfoProvider>(DependencyLifeStyle.Transient);

            Configuration.Modules.GeevAspNetCore().FormBodyBindingIgnoredTypes.Add(typeof(IFormFile));

            Configuration.MultiTenancy.Resolvers.Add<DomainTenantResolveContributor>();
            Configuration.MultiTenancy.Resolvers.Add<HttpHeaderTenantResolveContributor>();
            Configuration.MultiTenancy.Resolvers.Add<HttpCookieTenantResolveContributor>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevAspNetCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            AddApplicationParts();
            ConfigureAntiforgery();
        }

        private void AddApplicationParts()
        {
            var configuration = IocManager.Resolve<GeevAspNetCoreConfiguration>();
            var partManager = IocManager.Resolve<ApplicationPartManager>();
            var moduleManager = IocManager.Resolve<IGeevModuleManager>();

            var controllerAssemblies = configuration.ControllerAssemblySettings.Select(s => s.Assembly).Distinct();
            foreach (var controllerAssembly in controllerAssemblies)
            {
                partManager.ApplicationParts.Add(new AssemblyPart(controllerAssembly));
            }

            var plugInAssemblies = moduleManager.Modules.Where(m => m.IsLoadedAsPlugIn).Select(m => m.Assembly).Distinct();
            foreach (var plugInAssembly in plugInAssemblies)
            {
                partManager.ApplicationParts.Add(new AssemblyPart(plugInAssembly));
            }
        }

        private void ConfigureAntiforgery()
        {
            IocManager.Using<IOptions<AntiforgeryOptions>>(optionsAccessor =>
            {
                optionsAccessor.Value.HeaderName = Configuration.Modules.GeevWebCommon().AntiForgery.TokenHeaderName;
            });
        }
    }
}