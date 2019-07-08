using System;
using Geev.Configuration.Startup;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;

namespace Geev.WebApi.OData.Configuration
{
    internal class GeevWebApiODataModuleConfiguration : IGeevWebApiODataModuleConfiguration
    {
        public ODataConventionModelBuilder ODataModelBuilder { get; set; }

        public Action<IGeevStartupConfiguration> MapAction { get; set; }

        public GeevWebApiODataModuleConfiguration()
        {
            ODataModelBuilder = new ODataConventionModelBuilder();

            MapAction = configuration =>
            {
                configuration.Modules.GeevWebApi().HttpConfiguration.MapODataServiceRoute(
                    routeName: "ODataRoute",
                    routePrefix: "odata",
                    model: configuration.Modules.GeevWebApiOData().ODataModelBuilder.GetEdmModel()
                );
            };
        }
    }
}