using System.Collections.Generic;
using Geev.Web.Api.ProxyScripting.Configuration;
using Geev.Web.MultiTenancy;
using Geev.Web.Security.AntiForgery;

namespace Geev.Web.Configuration
{
    internal class GeevWebCommonModuleConfiguration : IGeevWebCommonModuleConfiguration
    {
        public bool SendAllExceptionsToClients { get; set; }

        public IApiProxyScriptingConfiguration ApiProxyScripting { get; }

        public IGeevAntiForgeryConfiguration AntiForgery { get; }

        public IWebEmbeddedResourcesConfiguration EmbeddedResources { get; }

        public IWebMultiTenancyConfiguration MultiTenancy { get; }

        public GeevWebCommonModuleConfiguration(
            IApiProxyScriptingConfiguration apiProxyScripting, 
            IGeevAntiForgeryConfiguration geevAntiForgery,
            IWebEmbeddedResourcesConfiguration embeddedResources, 
            IWebMultiTenancyConfiguration multiTenancy)
        {
            ApiProxyScripting = apiProxyScripting;
            AntiForgery = geevAntiForgery;
            EmbeddedResources = embeddedResources;
            MultiTenancy = multiTenancy;
        }
    }
}