using System.Reflection;
using Geev.Collections.Extensions;
using Geev.Dependency;
using Geev.Modules;
using Geev.WebApi.OData.Configuration;
using Microsoft.AspNet.OData;

namespace Geev.WebApi.OData
{
    [DependsOn(typeof(GeevWebApiModule))]
    public class GeevWebApiODataModule : GeevModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IGeevWebApiODataModuleConfiguration, GeevWebApiODataModuleConfiguration>();

            Configuration.Validation.IgnoredTypes.AddIfNotContains(typeof(Delta));
        }

        public override void Initialize()
        {
            IocManager.Register<MetadataController>(DependencyLifeStyle.Transient);
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.GeevWebApiOData().MapAction?.Invoke(Configuration);
        }
    }
}
