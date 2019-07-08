using System.Reflection;
using Geev.Modules;
using Geev.MongoDb.Configuration;

namespace Geev.MongoDb
{
    /// <summary>
    /// This module is used to implement "Data Access Layer" in MongoDB.
    /// </summary>
    [DependsOn(typeof(GeevKernelModule))]
    public class GeevMongoDbModule : GeevModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IGeevMongoDbModuleConfiguration, GeevMongoDbModuleConfiguration>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
