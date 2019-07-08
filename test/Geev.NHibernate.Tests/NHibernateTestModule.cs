using System;
using System.Data.Common;
using System.Reflection;
using Geev.Configuration.Startup;
using Geev.Modules;
using Geev.TestBase;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate.Tool.hbm2ddl;

namespace Geev.NHibernate.Tests
{
    [DependsOn(typeof(GeevNHibernateModule), typeof(GeevTestBaseModule))]
    public class NHibernateTestModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.GeevNHibernate().FluentConfiguration
                .Database(SQLiteConfiguration.Standard.InMemory())
                .Mappings(m =>
                    m.FluentMappings
                        .Conventions.Add(
                           DynamicInsert.AlwaysTrue(),
                           DynamicUpdate.AlwaysTrue()
                        )
                        .AddFromAssembly(Assembly.GetExecutingAssembly())
                ).ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(true, true, false, IocManager.Resolve<DbConnection>(), Console.Out));
        }
    }
}