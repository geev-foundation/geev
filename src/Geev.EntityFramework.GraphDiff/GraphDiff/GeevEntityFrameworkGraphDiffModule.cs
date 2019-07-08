using System.Collections.Generic;
using System.Reflection;
using Geev.EntityFramework.GraphDiff.Configuration;
using Geev.EntityFramework.GraphDiff.Mapping;
using Geev.Modules;

namespace Geev.EntityFramework.GraphDiff
{
    [DependsOn(typeof(GeevEntityFrameworkModule), typeof(GeevKernelModule))]
    public class GeevEntityFrameworkGraphDiffModule : GeevModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IGeevEntityFrameworkGraphDiffModuleConfiguration, GeevEntityFrameworkGraphDiffModuleConfiguration>();

            Configuration.Modules
                .GeevEfGraphDiff()
                .UseMappings(new List<EntityMapping>());
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
