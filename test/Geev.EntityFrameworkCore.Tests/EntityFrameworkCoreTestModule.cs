using System;
using System.Transactions;
using Geev.Domain.Repositories;
using Geev.EntityFrameworkCore.Tests.Domain;
using Geev.EntityFrameworkCore.Tests.Ef;
using Geev.Modules;
using Geev.TestBase;
using Castle.MicroKernel.Registration;
using Microsoft.EntityFrameworkCore;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Reflection.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Geev.EntityFrameworkCore.Tests
{
    [DependsOn(typeof(GeevEntityFrameworkCoreModule), typeof(GeevTestBaseModule))]
    public class EntityFrameworkCoreTestModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsolationLevel = IsolationLevel.Unspecified;

            //BloggingDbContext
            RegisterBloggingDbContextToSqliteInMemoryDb(IocManager);

            //SupportDbContext
            RegisterSupportDbContextToSqliteInMemoryDb(IocManager);
            
            //Custom repository
            Configuration.ReplaceService<IRepository<Post, Guid>>(() =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IRepository<Post, Guid>, IPostRepository, PostRepository>()
                        .ImplementedBy<PostRepository>()
                        .LifestyleTransient()
                );
            });

            Configuration.IocManager.Register<IRepository<TicketListItem>, TicketListItemRepository>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EntityFrameworkCoreTestModule).GetAssembly());
        }

        private static void RegisterBloggingDbContextToSqliteInMemoryDb(IIocManager iocManager)
        {
            var builder = new DbContextOptionsBuilder<BloggingDbContext>();

            builder.ReplaceService<IEntityMaterializerSource, GeevEntityMaterializerSource>();

            var inMemorySqlite = new SqliteConnection("Data Source=:memory:");
            builder.UseSqlite(inMemorySqlite);

            iocManager.IocContainer.Register(
                Component
                    .For<DbContextOptions<BloggingDbContext>>()
                    .Instance(builder.Options)
                    .LifestyleSingleton()
            );

            inMemorySqlite.Open();
            new BloggingDbContext(builder.Options).Database.EnsureCreated();
        }

        private static void RegisterSupportDbContextToSqliteInMemoryDb(IIocManager iocManager)
        {
            var builder = new DbContextOptionsBuilder<SupportDbContext>();

            builder.ReplaceService<IEntityMaterializerSource, GeevEntityMaterializerSource>();

            var inMemorySqlite = new SqliteConnection("Data Source=:memory:");
            builder.UseSqlite(inMemorySqlite);

            iocManager.IocContainer.Register(
                Component
                    .For<DbContextOptions<SupportDbContext>>()
                    .Instance(builder.Options)
                    .LifestyleSingleton()
            );

            inMemorySqlite.Open();
            var ctx = new SupportDbContext(builder.Options);
            ctx.Database.EnsureCreated();

            using (var command = ctx.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = SupportDbContext.TicketViewSql;
                ctx.Database.OpenConnection();

                command.ExecuteNonQuery();
            }
        }
    }
}