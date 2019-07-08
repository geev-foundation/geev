using System.Reflection;
using Geev.Collections.Extensions;
using Geev.Dependency;
using Geev.Modules;
using Geev.AspNetCore.OData.Configuration;
using Microsoft.AspNet.OData;

namespace Geev.AspNetCore.OData
{
    [DependsOn(typeof(GeevAspNetCoreModule))]
    public class GeevAspNetCoreODataModule : GeevModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IGeevAspNetCoreODataModuleConfiguration, GeevAspNetCoreODataModuleConfiguration>();

            Configuration.Validation.IgnoredTypes.AddIfNotContains(typeof(Delta));
        }

        public override void Initialize()
        {
            IocManager.Register<MetadataController>(DependencyLifeStyle.Transient);
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
