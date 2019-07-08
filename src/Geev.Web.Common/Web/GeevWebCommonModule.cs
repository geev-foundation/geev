using Geev.Configuration.Startup;
using Geev.Localization.Dictionaries;
using Geev.Localization.Dictionaries.Xml;
using Geev.Modules;
using Geev.Web.Api.ProxyScripting.Configuration;
using Geev.Web.Api.ProxyScripting.Generators.JQuery;
using Geev.Web.Configuration;
using Geev.Web.MultiTenancy;
using Geev.Web.Security.AntiForgery;
using Geev.Reflection.Extensions;
using Geev.Web.Minifier;

namespace Geev.Web
{
    /// <summary>
    /// This module is used to use ABP in ASP.NET web applications.
    /// </summary>
    [DependsOn(typeof(GeevKernelModule))]    
    public class GeevWebCommonModule : GeevModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            IocManager.Register<IWebMultiTenancyConfiguration, WebMultiTenancyConfiguration>();
            IocManager.Register<IApiProxyScriptingConfiguration, ApiProxyScriptingConfiguration>();
            IocManager.Register<IGeevAntiForgeryConfiguration, GeevAntiForgeryConfiguration>();
            IocManager.Register<IWebEmbeddedResourcesConfiguration, WebEmbeddedResourcesConfiguration>();
            IocManager.Register<IGeevWebCommonModuleConfiguration, GeevWebCommonModuleConfiguration>();
            IocManager.Register<IJavaScriptMinifier, NUglifyJavaScriptMinifier>();

            Configuration.Modules.GeevWebCommon().ApiProxyScripting.Generators[JQueryProxyScriptGenerator.Name] = typeof(JQueryProxyScriptGenerator);

            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    GeevWebConsts.LocalizaionSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(GeevWebCommonModule).GetAssembly(), "Geev.Web.Localization.GeevWebXmlSource"
                        )));
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevWebCommonModule).GetAssembly());            
        }
    }
}
