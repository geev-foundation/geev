using System.Reflection;
using Geev.MemoryDb.Configuration;
using Geev.Modules;
using Geev.Reflection.Extensions;

namespace Geev.MemoryDb
{
    /// <summary>
    /// This module is used to implement "Data Access Layer" in MemoryDb.
    /// </summary>
    public class GeevMemoryDbModule : GeevModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IGeevMemoryDbModuleConfiguration, GeevMemoryDbModuleConfiguration>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevMemoryDbModule).GetAssembly());
        }
    }
}
