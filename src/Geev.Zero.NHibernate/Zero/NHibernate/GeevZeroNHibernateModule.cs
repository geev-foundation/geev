using System.Reflection;
using Geev.Configuration.Startup;
using Geev.Modules;
using Geev.NHibernate;

namespace Geev.Zero.NHibernate
{
    /// <summary>
    /// Startup class for ABP Zero NHibernate module.
    /// </summary>
    [DependsOn(typeof(GeevZeroCoreModule), typeof(GeevNHibernateModule))]
    public class GeevZeroNHibernateModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.GeevNHibernate().FluentConfiguration
                .Mappings(
                    m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly())
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
