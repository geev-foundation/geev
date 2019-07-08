using Geev.Domain.Uow;
using Geev.EntityFrameworkCore;
using Geev.Modules;
using Geev.MultiTenancy;
using Geev.Reflection.Extensions;
using Castle.MicroKernel.Registration;

namespace Geev.Zero.EntityFrameworkCore
{
    /// <summary>
    /// Entity framework integration module for ASP.NET Boilerplate Zero.
    /// </summary>
    [DependsOn(typeof(GeevZeroCoreModule), typeof(GeevEntityFrameworkCoreModule))]
    public class GeevZeroCoreEntityFrameworkCoreModule : GeevModule
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
            IocManager.RegisterAssemblyByConvention(typeof(GeevZeroCoreEntityFrameworkCoreModule).GetAssembly());
        }
    }
}
