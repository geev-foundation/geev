using System.Collections.Generic;
using System.Reflection;
using Geev.Domain.Uow;
using Geev.Modules;
using Geev.Runtime.Session;
using Geev.TestBase;
using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Geev.Zero.SampleApp.EntityFrameworkCore.Tests
{
    [DependsOn(
        typeof(SampleAppEntityFrameworkCoreModule),
        typeof(GeevTestBaseModule))]
    public class SampleAppTestModule : GeevModule
    {
        private DbContextOptions<AppDbContext> _hostDbContextOptions;
        private Dictionary<int, DbContextOptions<AppDbContext>> _tenantDbContextOptions;
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            SetupInMemoryDb();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        private void SetupInMemoryDb()
        {
            var services = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase();

            var serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(
                IocManager.IocContainer,
                services
            );

            var hostDbContextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            hostDbContextOptionsBuilder.UseInMemoryDatabase().UseInternalServiceProvider(serviceProvider);

            _hostDbContextOptions = hostDbContextOptionsBuilder.Options;
            _tenantDbContextOptions = new Dictionary<int, DbContextOptions<AppDbContext>>();

            IocManager.IocContainer.Register(
                Component
                    .For<DbContextOptions<AppDbContext>>()
                    .UsingFactoryMethod((kernel) =>
                    {
                        lock (_tenantDbContextOptions)
                        {
                            var currentUow = kernel.Resolve<ICurrentUnitOfWorkProvider>().Current;
                            var geevSession = kernel.Resolve<IGeevSession>();

                            var tenantId = currentUow != null ? currentUow.GetTenantId() : geevSession.TenantId;

                            if (tenantId == null)
                            {
                                return _hostDbContextOptions;
                            }

                            if (!_tenantDbContextOptions.ContainsKey(tenantId.Value))
                            {
                                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                                optionsBuilder.UseInMemoryDatabase(tenantId.Value.ToString()).UseInternalServiceProvider(serviceProvider);
                                _tenantDbContextOptions[tenantId.Value] = optionsBuilder.Options;
                            }

                            return _tenantDbContextOptions[tenantId.Value];
                        }
                    }, true)
                    .LifestyleTransient()
            );
        }
    }
}