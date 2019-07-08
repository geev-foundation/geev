using Geev.Configuration.Startup;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Geev.AspNetCore.OData.Configuration
{
    public static class GeevAspNetCoreODataApplicationBuilderExtensions
    {
        public static void UseOData(this IApplicationBuilder app, Action<ODataConventionModelBuilder> builderAction)
        {
            var configuration = app.ApplicationServices.GetService<IGeevStartupConfiguration>();

            if (configuration.Modules.GeevAspNetCoreOData().ODataModelBuilder == null)
            {
                configuration.Modules.GeevAspNetCoreOData().ODataModelBuilder = new ODataConventionModelBuilder(app.ApplicationServices);
            }

            builderAction(configuration.Modules.GeevAspNetCoreOData().ODataModelBuilder);
        }
    }
}
