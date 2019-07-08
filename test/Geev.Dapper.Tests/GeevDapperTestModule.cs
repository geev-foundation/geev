using System.Collections.Generic;
using System.Reflection;
using System.Transactions;

using Geev.EntityFramework;
using Geev.Modules;
using Geev.TestBase;

using DapperExtensions.Sql;

namespace Geev.Dapper.Tests
{
    [DependsOn(
        typeof(GeevEntityFrameworkModule),
        typeof(GeevTestBaseModule),
        typeof(GeevDapperModule)
    )]
    public class GeevDapperTestModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsolationLevel = IsolationLevel.Unspecified;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();

            DapperExtensions.DapperExtensions.SetMappingAssemblies(new List<Assembly> { Assembly.GetExecutingAssembly() });
        }
    }
}
