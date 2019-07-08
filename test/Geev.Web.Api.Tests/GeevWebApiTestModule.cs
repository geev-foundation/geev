using System.Reflection;
using Geev.Application.Services;
using Geev.Configuration.Startup;
using Geev.Modules;
using Geev.TestBase;
using Geev.Web.Api.Tests.AppServices;
using Geev.WebApi;

namespace Geev.Web.Api.Tests
{
    [DependsOn(typeof(GeevWebApiModule), typeof(GeevTestBaseModule))]
    public class GeevWebApiTestModule : GeevModule
    {
        public override void PreInitialize()
        {
            base.PreInitialize();

            Configuration.Localization.IsEnabled = false;
        }

        public override void Initialize()
        {
            base.Initialize();

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.GeevWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(Assembly.GetExecutingAssembly(), "myapp")
                .ForMethods(builder =>
                {
                    if (builder.Method.IsDefined(typeof(MyIgnoreApiAttribute)))
                    {
                        builder.DontCreate = true;
                    }
                })
                .WithProxyScripts(false)
                .Build();
        }
    }
}