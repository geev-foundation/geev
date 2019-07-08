using System;
using System.Data.Common;
using System.Reflection;
using Geev.Configuration.Startup;
using Geev.Modules;
using Geev.Zero.NHibernate;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;

namespace Geev.Zero.SampleApp.NHibernate
{
    [DependsOn(typeof(GeevZeroNHibernateModule), typeof(SampleAppModule))]
    public class SampleAppNHibernateModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.GeevNHibernate().FluentConfiguration
                .Database(SQLiteConfiguration.Standard.InMemory())
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(true, true, false, IocManager.Resolve<DbConnection>(), Console.Out));
        }
    }
}
