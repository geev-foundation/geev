using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SQLite;
using System.Data.SQLite.EF6;

using Geev.Dapper.Tests.Entities;
using Geev.EntityFramework;

namespace Geev.Dapper.Tests.DbContexts
{
    [DbConfigurationType(typeof(DapperDbContextConfiguration3))]
    public class SampleApplicationDbContext3 : GeevDbContext
    {
        public SampleApplicationDbContext3(DbConnection connection)
            : base(connection, false)
        {
        }

        public SampleApplicationDbContext3(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {
        }

        public virtual IDbSet<Person> Person { get; set; }

        public virtual IDbSet<Good> Goods { get; set; }
    }

    public class DapperDbContextConfiguration3 : DbConfiguration
    {
        public DapperDbContextConfiguration3()
        {
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderServices("System.Data.SQLite", (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));
        }
    }
}
