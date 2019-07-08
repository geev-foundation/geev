using System.Reflection;
using Geev.Domain.Uow;
using Geev.EntityFramework;
using Geev.Modules;
using Geev.MultiTenancy;
using Castle.MicroKernel.Registration;

namespace Geev.Zero.EntityFramework
{
    /// <summary>
    /// Entity framework integration module for ASP.NET Boilerplate Zero.
    /// </summary>
    [DependsOn(typeof(GeevZeroCoreModule), typeof(GeevEntityFrameworkModule))]
    public class GeevZeroEntityFrameworkModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.ReplaceService(typeof(IConnectionStringResolver), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IConnectionStringResolver, IDbPerTenantConnectionStringResolver>()
                        .ImplementedBy<DbPerTenantConnectionStringResolver>()
                        .LifestyleTransient()
                    );
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
