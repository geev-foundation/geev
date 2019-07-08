using System;
using System.Threading.Tasks;
using Geev.AspNetCore.TestBase;
using Geev.Domain.Uow;
using Geev.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;
using Xunit;

namespace Geev.AspNetCore.Tests.Uow
{
    public class UnitOfWorkMiddleware_Tests : GeevAspNetCoreIntegratedTestBase<UnitOfWorkMiddleware_Tests.Startup>
    {
        [Fact]
        public async Task Current_UnitOfWork_Should_Be_Available_After_UnitOfWork_Middleware()
        {
            var response = await Client.GetAsync("/");
            var str = await response.Content.ReadAsStringAsync();
            str.ShouldBe("not-null");
        }

        public class Startup
        {
            public IServiceProvider ConfigureServices(IServiceCollection services)
            {
                return services.AddGeev<StartupModule>(options =>
                {
                    options.SetupTest();
                });
            }

            public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
            {
                app.UseGeev();

                app.UseUnitOfWork();

                app.Use(async (context, func) =>
                {
                    await context.Response.WriteAsync(
                        context.RequestServices.GetRequiredService<IUnitOfWorkManager>().Current == null
                            ? "null"
                            : "not-null"
                    );
                });
            }
        }

        public class StartupModule : GeevModule
        {
            
        }
    }
}
