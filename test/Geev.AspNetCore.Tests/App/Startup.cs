using System;
using Geev.AspNetCore.Configuration;
using Geev.AspNetCore.Mvc.Extensions;
using Geev.AspNetCore.TestBase;
using Geev.Reflection.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Geev.AspNetCore.App
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var mvc = services.AddMvc().AddXmlSerializerFormatters();

            mvc.PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GeevAspNetCoreModule).GetAssembly()));

            //Configure Geev and Dependency Injection
            return services.AddGeev<AppModule>(options =>
            {
                //Test setup
                options.SetupTest();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseGeev(); //Initializes ABP framework.

            app.UseMvc(routes =>
            {
                app.ApplicationServices.GetRequiredService<IGeevAspNetCoreConfiguration>().RouteConfiguration.ConfigureAll(routes);
            });
        }
    }
}
