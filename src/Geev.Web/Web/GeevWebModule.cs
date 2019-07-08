using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Geev.Auditing;
using Geev.Modules;
using Geev.Runtime.Session;
using Geev.Web.Session;
using Geev.Configuration.Startup;
using Geev.Web.Configuration;
using Geev.Web.Security.AntiForgery;
using Geev.Collections.Extensions;
using Geev.Dependency;
using Geev.Web.MultiTenancy;

namespace Geev.Web
{
    /// <summary>
    /// This module is used to use ABP in ASP.NET web applications.
    /// </summary>
    [DependsOn(typeof(GeevWebCommonModule))]    
    public class GeevWebModule : GeevModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            IocManager.Register<IGeevAntiForgeryWebConfiguration, GeevAntiForgeryWebConfiguration>();
            IocManager.Register<IGeevWebLocalizationConfiguration, GeevWebLocalizationConfiguration>();
            IocManager.Register<IGeevWebModuleConfiguration, GeevWebModuleConfiguration>();
            
            Configuration.ReplaceService<IPrincipalAccessor, HttpContextPrincipalAccessor>(DependencyLifeStyle.Transient);
            Configuration.ReplaceService<IClientInfoProvider, WebClientInfoProvider>(DependencyLifeStyle.Transient);

            Configuration.MultiTenancy.Resolvers.Add<DomainTenantResolveContributor>();
            Configuration.MultiTenancy.Resolvers.Add<HttpHeaderTenantResolveContributor>();
            Configuration.MultiTenancy.Resolvers.Add<HttpCookieTenantResolveContributor>();

            AddIgnoredTypes();
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());            
        }

        private void AddIgnoredTypes()
        {
            var ignoredTypes = new[]
            {
                typeof(HttpPostedFileBase),
                typeof(IEnumerable<HttpPostedFileBase>),
                typeof(HttpPostedFileWrapper),
                typeof(IEnumerable<HttpPostedFileWrapper>)
            };
            
            foreach (var ignoredType in ignoredTypes)
            {
                Configuration.Auditing.IgnoredTypes.AddIfNotContains(ignoredType);
                Configuration.Validation.IgnoredTypes.AddIfNotContains(ignoredType);
            }
        }
    }
}
