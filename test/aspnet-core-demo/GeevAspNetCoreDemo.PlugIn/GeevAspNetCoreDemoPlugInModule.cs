using System.Reflection;
using Geev.AspNetCore;
using Geev.Modules;
using Geev.Reflection.Extensions;
using Geev.Resources.Embedded;

namespace GeevAspNetCoreDemo.PlugIn
{
    [DependsOn(typeof(GeevAspNetCoreModule))]
    public class GeevAspNetCoreDemoPlugInModule : GeevModule
    {
        public override void PreInitialize()
        {

            Configuration.EmbeddedResources.Sources.Add(
                new EmbeddedResourceSet(
                    "/Views/",
                    typeof(GeevAspNetCoreDemoPlugInModule).GetAssembly(),
                    "GeevAspNetCoreDemo.PlugIn.Views"
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevAspNetCoreDemoPlugInModule).GetAssembly());
        }
    }
}
