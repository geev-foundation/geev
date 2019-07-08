using System;
using System.Collections.Generic;
using System.Reflection;
using System.Transactions;
using Geev.Configuration.Startup;
using Geev.Dapper;
using Geev.Domain.Repositories;
using Geev.EntityFrameworkCore.Dapper.Tests.Domain;
using Geev.EntityFrameworkCore.Dapper.Tests.Ef;
using Geev.Modules;
using Geev.Reflection.Extensions;
using Geev.TestBase;
using Castle.MicroKernel.Registration;
using DapperExtensions.Sql;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Geev.EntityFrameworkCore.Dapper.Tests
{
    [DependsOn(
        typeof(GeevEntityFrameworkCoreModule),
        typeof(GeevDapperModule),
        typeof(GeevTestBaseModule))]
    public class GeevEfCoreDapperTestModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsolationLevel = IsolationLevel.Unspecified;

            DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();
            
            Configuration.ReplaceService<IRepository<Post, Guid>>(() =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IRepository<Post, Guid>, IPostRepository, PostRepository>()
                             .ImplementedBy<PostRepository>()
                             .LifestyleTransient()
                );
            });
        }

        public override void Initialize()
        {
            var builder = new DbContextOptionsBuilder<BloggingDbContext>();

            var inMemorySqlite = new SqliteConnection("Data Source=:memory:");
            builder.UseSqlite(inMemorySqlite);

            IocManager.IocContainer.Register(
                Component
                    .For<DbContextOptions<BloggingDbContext>>()
                    .Instance(builder.Options)
                    .LifestyleSingleton()
            );

            inMemorySqlite.Open();
            new BloggingDbContext(builder.Options).Database.EnsureCreated();

            IocManager.RegisterAssemblyByConvention(typeof(GeevEfCoreDapperTestModule).GetAssembly());

            DapperExtensions.DapperExtensions.SetMappingAssemblies(new List<Assembly> { typeof(GeevEfCoreDapperTestModule).GetAssembly() });
        }
    }
}
