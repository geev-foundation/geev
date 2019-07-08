using System;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Mvc;
using Geev.Configuration.Startup;
using Geev.Modules;
using Geev.Web.Mvc.Auditing;
using Geev.Web.Mvc.Authorization;
using Geev.Web.Mvc.Configuration;
using Geev.Web.Mvc.Controllers;
using Geev.Web.Mvc.ModelBinding.Binders;
using Geev.Web.Mvc.Resources.Embedded;
using Geev.Web.Mvc.Security.AntiForgery;
using Geev.Web.Mvc.Uow;
using Geev.Web.Mvc.Validation;
using Geev.Web.Security.AntiForgery;

namespace Geev.Web.Mvc
{
    /// <summary>
    /// This module is used to build ASP.NET MVC web sites using Geev.
    /// </summary>
    [DependsOn(typeof(GeevWebModule))]
    public class GeevWebMvcModule : GeevModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new ControllerConventionalRegistrar());

            IocManager.Register<IGeevMvcConfiguration, GeevMvcConfiguration>();

            Configuration.ReplaceService<IGeevAntiForgeryManager, GeevMvcAntiForgeryManager>();
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(IocManager));
            HostingEnvironment.RegisterVirtualPathProvider(IocManager.Resolve<EmbeddedResourceVirtualPathProvider>());
        }

        /// <inheritdoc/>
        public override void PostInitialize()
        {
            GlobalFilters.Filters.Add(IocManager.Resolve<GeevMvcAuthorizeFilter>());
            GlobalFilters.Filters.Add(IocManager.Resolve<GeevAntiForgeryMvcFilter>());
            GlobalFilters.Filters.Add(IocManager.Resolve<GeevMvcAuditFilter>());
            GlobalFilters.Filters.Add(IocManager.Resolve<GeevMvcValidationFilter>());
            GlobalFilters.Filters.Add(IocManager.Resolve<GeevMvcUowFilter>());

            var geevMvcDateTimeBinder = new GeevMvcDateTimeBinder();
            ModelBinders.Binders.Add(typeof(DateTime), geevMvcDateTimeBinder);
            ModelBinders.Binders.Add(typeof(DateTime?), geevMvcDateTimeBinder);
        }
    }
}
