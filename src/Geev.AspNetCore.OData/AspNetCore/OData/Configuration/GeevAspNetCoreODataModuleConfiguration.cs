using System;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Routing;

namespace Geev.AspNetCore.OData.Configuration
{
    internal class GeevAspNetCoreODataModuleConfiguration : IGeevAspNetCoreODataModuleConfiguration
    {
        public ODataConventionModelBuilder ODataModelBuilder { get; set; }

        public Action<IRouteBuilder> MapAction { get; set; }

        public GeevAspNetCoreODataModuleConfiguration()
        {
            MapAction = routes =>
            {
                routes.MapODataServiceRoute(
                    routeName: "ODataRoute",
                    routePrefix: "odata",
                    model: ODataModelBuilder.GetEdmModel()
                );
            };
        }
    }
}
