using Geev.Domain.Uow;
using Geev.EntityFramework;
using Geev.Modules;
using Geev.MultiTenancy;
using Geev.Reflection.Extensions;
using Castle.MicroKernel.Registration;

namespace Geev.Zero.EntityFramework
{
    /// <summary>
    /// Entity framework integration module for ASP.NET Boilerplate Zero.
    /// </summary>
    [DependsOn(typeof(GeevZeroCoreModule), typeof(GeevEntityFrameworkModule))]
    public class GeevZeroCoreEntityFrameworkModule : GeevModule
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
            IocManager.RegisterAssemblyByConvention(typeof(GeevZeroCoreEntityFrameworkModule).GetAssembly());
        }
    }
}
