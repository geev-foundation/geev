using Geev.AspNetCore;
using Geev.AspNetCore.Configuration;
using Geev.AspNetCore.OData;
using Geev.Castle.Logging.Log4Net;
using Geev.Dependency;
using Geev.EntityFrameworkCore;
using Geev.Modules;
using Geev.Reflection.Extensions;
using GeevAspNetCoreDemo.Core;
using GeevAspNetCoreDemo.Db;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GeevAspNetCoreDemo
{
    [DependsOn(
        typeof(GeevAspNetCoreModule),
        typeof(GeevAspNetCoreDemoCoreModule),
        typeof(GeevEntityFrameworkCoreModule),
        typeof(GeevCastleLog4NetModule),
        typeof(GeevAspNetCoreODataModule)
        )]
    public class GeevAspNetCoreDemoModule : GeevModule
    {
        public override void PreInitialize()
        {
            RegisterDbContextToSqliteInMemoryDb(IocManager);

            Configuration.Modules.GeevAspNetCore()
                .CreateControllersForAppServices(
                    typeof(GeevAspNetCoreDemoCoreModule).GetAssembly()
                );


            Configuration.IocManager.Resolve<IGeevAspNetCoreConfiguration>().RouteConfiguration.Add(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevAspNetCoreDemoModule).GetAssembly());
        }

        private static void RegisterDbContextToSqliteInMemoryDb(IIocManager iocManager)
        {
            var builder = new DbContextOptionsBuilder<MyDbContext>();

            builder.ReplaceService<IEntityMaterializerSource, GeevEntityMaterializerSource>();

            var inMemorySqlite = new SqliteConnection("Data Source=:memory:");
            builder.UseSqlite(inMemorySqlite);

            iocManager.IocContainer.Register(
                Component
                    .For<DbContextOptions<MyDbContext>>()
                    .Instance(builder.Options)
                    .LifestyleSingleton()
            );

            inMemorySqlite.Open();
            var ctx = new MyDbContext(builder.Options);
            ctx.Database.EnsureCreated();
        }
    }
}