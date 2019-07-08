using System;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Routing;

namespace Geev.AspNetCore.OData.Configuration
{
    /// <summary>
    /// Used to configure Geev.AspNetCore.OData module.
    /// </summary>
    public interface IGeevAspNetCoreODataModuleConfiguration
    {
        /// <summary>
        /// Gets ODataConventionModelBuilder.
        /// </summary>
        ODataConventionModelBuilder ODataModelBuilder { get; set; }

        /// <summary>
        /// Allows overriding OData mapping.
        /// </summary>
        Action<IRouteBuilder> MapAction { get; set; }
    }
}
