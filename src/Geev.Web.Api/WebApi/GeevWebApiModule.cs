using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Geev.Logging;
using Geev.Modules;
using Geev.Web;
using Geev.WebApi.Configuration;
using Geev.WebApi.Controllers;
using Geev.WebApi.Controllers.Dynamic;
using Geev.WebApi.Controllers.Dynamic.Formatters;
using Geev.WebApi.Controllers.Dynamic.Selectors;
using Geev.WebApi.Runtime.Caching;
using Castle.MicroKernel.Registration;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using Geev.Configuration.Startup;
using Geev.Json;
using Geev.WebApi.Auditing;
using Geev.WebApi.Authorization;
using Geev.WebApi.Controllers.ApiExplorer;
using Geev.WebApi.Controllers.Dynamic.Binders;
using Geev.WebApi.Controllers.Dynamic.Builders;
using Geev.WebApi.ExceptionHandling;
using Geev.WebApi.Security.AntiForgery;
using Geev.WebApi.Uow;
using Geev.WebApi.Validation;

namespace Geev.WebApi
{
    /// <summary>
    /// This module provides Geev features for ASP.NET Web API.
    /// </summary>
    [DependsOn(typeof(GeevWebModule))]
    public class GeevWebApiModule : GeevModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new ApiControllerConventionalRegistrar());

            IocManager.Register<IDynamicApiControllerBuilder, DynamicApiControllerBuilder>();
            IocManager.Register<IGeevWebApiConfiguration, GeevWebApiConfiguration>();

            Configuration.Settings.Providers.Add<ClearCacheSettingProvider>();

            Configuration.Modules.GeevWebApi().ResultWrappingIgnoreUrls.Add("/swagger");
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            var httpConfiguration = IocManager.Resolve<IGeevWebApiConfiguration>().HttpConfiguration;

            InitializeAspNetServices(httpConfiguration);
            InitializeFilters(httpConfiguration);
            InitializeFormatters(httpConfiguration);
            InitializeRoutes(httpConfiguration);
            InitializeModelBinders(httpConfiguration);

            foreach (var controllerInfo in IocManager.Resolve<DynamicApiControllerManager>().GetAll())
            {
                IocManager.IocContainer.Register(
                    Component.For(controllerInfo.InterceptorType).LifestyleTransient(),
                    Component.For(controllerInfo.ApiControllerType)
                        .Proxy.AdditionalInterfaces(controllerInfo.ServiceInterfaceType)
                        .Interceptors(controllerInfo.InterceptorType)
                        .LifestyleTransient()
                    );

                LogHelper.Logger.DebugFormat("Dynamic web api controller is created for type '{0}' with service name '{1}'.", controllerInfo.ServiceInterfaceType.FullName, controllerInfo.ServiceName);
            }

            Configuration.Modules.GeevWebApi().HttpConfiguration.EnsureInitialized();
        }

        private void InitializeAspNetServices(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.Services.Replace(typeof(IHttpControllerSelector), new GeevHttpControllerSelector(httpConfiguration, IocManager.Resolve<DynamicApiControllerManager>()));
            httpConfiguration.Services.Replace(typeof(IHttpActionSelector), new GeevApiControllerActionSelector(IocManager.Resolve<IGeevWebApiConfiguration>()));
            httpConfiguration.Services.Replace(typeof(IHttpControllerActivator), new GeevApiControllerActivator(IocManager));
            httpConfiguration.Services.Replace(typeof(IApiExplorer), IocManager.Resolve<GeevApiExplorer>());
        }

        private void InitializeFilters(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.Filters.Add(IocManager.Resolve<GeevApiAuthorizeFilter>());
            httpConfiguration.Filters.Add(IocManager.Resolve<GeevAntiForgeryApiFilter>());
            httpConfiguration.Filters.Add(IocManager.Resolve<GeevApiAuditFilter>());
            httpConfiguration.Filters.Add(IocManager.Resolve<GeevApiValidationFilter>());
            httpConfiguration.Filters.Add(IocManager.Resolve<GeevApiUowFilter>());
            httpConfiguration.Filters.Add(IocManager.Resolve<GeevApiExceptionFilterAttribute>());

            httpConfiguration.MessageHandlers.Add(IocManager.Resolve<ResultWrapperHandler>());
        }

        private static void InitializeFormatters(HttpConfiguration httpConfiguration)
        {
            //Remove formatters except JsonFormatter.
            foreach (var currentFormatter in httpConfiguration.Formatters.ToList())
            {
                if (!(currentFormatter is JsonMediaTypeFormatter || 
                    currentFormatter is JQueryMvcFormUrlEncodedFormatter))
                {
                    httpConfiguration.Formatters.Remove(currentFormatter);
                }
            }

            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new GeevCamelCasePropertyNamesContractResolver();
            httpConfiguration.Formatters.Add(new PlainTextFormatter());
        }

        private static void InitializeRoutes(HttpConfiguration httpConfiguration)
        {
            //Dynamic Web APIs

            httpConfiguration.Routes.MapHttpRoute(
                name: "GeevDynamicWebApi",
                routeTemplate: "api/services/{*serviceNameWithAction}"
                );

            //Other routes

            httpConfiguration.Routes.MapHttpRoute(
                name: "GeevCacheController_Clear",
                routeTemplate: "api/GeevCache/Clear",
                defaults: new { controller = "GeevCache", action = "Clear" }
                );

            httpConfiguration.Routes.MapHttpRoute(
                name: "GeevCacheController_ClearAll",
                routeTemplate: "api/GeevCache/ClearAll",
                defaults: new { controller = "GeevCache", action = "ClearAll" }
                );
        }

        private static void InitializeModelBinders(HttpConfiguration httpConfiguration)
        {
            var geevApiDateTimeBinder = new GeevApiDateTimeBinder();
            httpConfiguration.BindParameter(typeof(DateTime), geevApiDateTimeBinder);
            httpConfiguration.BindParameter(typeof(DateTime?), geevApiDateTimeBinder);
        }
    }
}
