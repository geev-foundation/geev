using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Geev.FluentValidation;
using Geev.Modules;
using Geev.Web.Mvc;

namespace GeevAspNetMvcDemo
{
     /// <summary>
    /// Web module of the application.
    /// This is the most top and entrance module that depends on others.
    /// </summary>
    [DependsOn(
        typeof(GeevWebMvcModule),
        typeof(GeevFluentValidationModule)
    )]
    public class GeevAspNetMvcDemoModule : GeevModule
    {
        public override void PreInitialize()
        {
            
        }

        public override void Initialize()
        {
            //Dependency Injection
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //Areas
            AreaRegistration.RegisterAllAreas();

            //Routes
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Bundling
            BundleTable.Bundles.IgnoreList.Clear();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}