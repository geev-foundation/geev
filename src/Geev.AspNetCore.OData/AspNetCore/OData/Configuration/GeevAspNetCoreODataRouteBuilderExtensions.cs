using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Geev.AspNetCore.OData.Configuration
{
    public static class GeevAspNetCoreODataRouteBuilderExtensions
    {
        public static void MapODataServiceRoute(this IRouteBuilder routes, IApplicationBuilder app)
        {
            var configuration = app.ApplicationServices.GetService<IGeevAspNetCoreODataModuleConfiguration>();

            configuration.MapAction(routes);
        }
    }
}
