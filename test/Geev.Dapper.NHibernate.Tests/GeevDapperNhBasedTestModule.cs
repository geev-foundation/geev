using System;
using System.Data.Common;
using System.Reflection;

using Geev.Configuration.Startup;
using Geev.Modules;
using Geev.NHibernate;
using Geev.TestBase;

using DapperExtensions.Sql;

using FluentNHibernate.Cfg.Db;

using NHibernate.Tool.hbm2ddl;

namespace Geev.Dapper.NHibernate.Tests
{
    [DependsOn(
        typeof(GeevNHibernateModule),
        typeof(GeevDapperModule),
        typeof(GeevTestBaseModule)
    )]
    public class GeevDapperNhBasedTestModule : GeevModule
    {
        private readonly object _lockObject = new object();

        public override void PreInitialize()
        {
            lock (_lockObject)
            {
                DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();
            }

            Configuration.Modules.GeevNHibernate().FluentConfiguration
                         .Database(SQLiteConfiguration.Standard.InMemory())
                         .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                         .ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(true, true, false, IocManager.Resolve<DbConnection>(), Console.Out));
        }
    }
}
