using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Geev.Castle.Logging.Log4Net;
using Geev.Dependency;
using Geev.Domain.Uow;
using Geev.EntityFrameworkCore;
using Geev.EntityFrameworkCore.Configuration;
using Geev.Modules;
using Geev.MultiTenancy;
using Geev.Zero.EntityFrameworkCore;
using Geev.Zero.SampleApp.EntityFrameworkCore.TestDataBuilders.HostDatas;
using Geev.Zero.SampleApp.EntityFrameworkCore.TestDataBuilders.TenantDatas;
using Geev.Zero.SampleApp.MultiTenancy;
using Castle.Facilities.Logging;
using Castle.LoggingFacility.MsLogging;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ILoggerFactory = Castle.Core.Logging.ILoggerFactory;

namespace Geev.Zero.SampleApp.EntityFrameworkCore.ConsoleAppTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var geevBootstrapper = GeevBootstrapper.Create<EfCoreTestConsoleAppModule>())
            {
                geevBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseGeevLog4Net().WithConfig("log4net.config")
                );

                geevBootstrapper.Initialize();
                geevBootstrapper.IocManager.Using<MigratorRunner>(migrateTester => migrateTester.Run());
            }

            Console.WriteLine("Press Enter to EXIT!");
            Console.ReadLine();
        }
    }

    [DependsOn(typeof(SampleAppEntityFrameworkCoreModule))]
    public class EfCoreTestConsoleAppModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Server=localhost; Database=GeevZeroMigrateTest; Trusted_Connection=True;";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddEntityFrameworkSqlServer();

            var serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(
                IocManager.IocContainer,
                services
            );

            var castleLoggerFactory = serviceProvider.GetService<ILoggerFactory>();
            if (castleLoggerFactory != null)
            {
                serviceProvider
                    .GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>()
                    .AddCastleLogger(castleLoggerFactory);
            }

            Configuration.Modules.GeevEfCore().AddDbContext<AppDbContext>(configuration =>
            {
                configuration.DbContextOptions
                    .UseInternalServiceProvider(serviceProvider)
                    .UseSqlServer(configuration.ConnectionString);
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }

    public class MigratorRunner : ITransientDependency
    {
        private readonly AppTestMigrator _appTestMigrator;

        public MigratorRunner(AppTestMigrator appTestMigrator)
        {
            _appTestMigrator = appTestMigrator;
        }

        public void Run()
        {
            List<Tenant> tenants = null;

            _appTestMigrator.CreateOrMigrateForHost(context =>
            {
                new HostDataBuilder(context).Build();
                tenants = context.Tenants.ToList();
            });

            foreach (var tenant in tenants)
            {
                _appTestMigrator.CreateOrMigrateForTenant(tenant, context =>
                {
                    new TenantDataBuilder(context).Build(tenant.Id);
                });
            }
        }
    }

    public class AppTestMigrator : GeevZeroDbMigrator<AppDbContext>
    {
        public AppTestMigrator(
            IUnitOfWorkManager unitOfWorkManager,
            IDbPerTenantConnectionStringResolver connectionStringResolver,
            IDbContextResolver dbContextResolver)
            : base(unitOfWorkManager,
                  connectionStringResolver,
                  dbContextResolver)
        {

        }
    }
}
