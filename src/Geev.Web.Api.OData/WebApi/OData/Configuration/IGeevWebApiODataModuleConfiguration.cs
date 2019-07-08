using System;
using Geev.Configuration.Startup;
using Microsoft.AspNet.OData.Builder;

namespace Geev.WebApi.OData.Configuration
{
    /// <summary>
    /// Used to configure Geev.Web.Api.OData module.
    /// </summary>
    public interface IGeevWebApiODataModuleConfiguration
    {
        /// <summary>
        /// Gets ODataConventionModelBuilder.
        /// </summary>
        ODataConventionModelBuilder ODataModelBuilder { get; set; }

        /// <summary>
        /// Allows overriding OData mapping.
        /// </summary>
        Action<IGeevStartupConfiguration> MapAction { get; set; }
    }
}