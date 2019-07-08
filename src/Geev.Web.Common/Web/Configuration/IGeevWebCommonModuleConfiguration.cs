using System.Collections.Generic;
using Geev.Web.Api.ProxyScripting.Configuration;
using Geev.Web.MultiTenancy;
using Geev.Web.Security.AntiForgery;

namespace Geev.Web.Configuration
{
    /// <summary>
    /// Used to configure ABP Web Common module.
    /// </summary>
    public interface IGeevWebCommonModuleConfiguration
    {
        /// <summary>
        /// If this is set to true, all exception and details are sent directly to clients on an error.
        /// Default: false (ABP hides exception details from clients except special exceptions.)
        /// </summary>
        bool SendAllExceptionsToClients { get; set; }

        /// <summary>
        /// Used to configure Api proxy scripting.
        /// </summary>
        IApiProxyScriptingConfiguration ApiProxyScripting { get; }

        /// <summary>
        /// Used to configure Anti Forgery security settings.
        /// </summary>
        IGeevAntiForgeryConfiguration AntiForgery { get; }

        /// <summary>
        /// Used to configure embedded resource system for web applications.
        /// </summary>
        IWebEmbeddedResourcesConfiguration EmbeddedResources { get; }

        IWebMultiTenancyConfiguration MultiTenancy { get; }
    }
}