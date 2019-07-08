using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SQLite;
using System.Data.SQLite.EF6;

using Geev.Dapper.Tests.Entities;
using Geev.EntityFramework;

namespace Geev.Dapper.Tests.DbContexts
{
    [DbConfigurationType(typeof(DapperDbContextConfiguration))]
    public class SampleDapperApplicationDbContext : GeevDbContext
    {
        public SampleDapperApplicationDbContext(DbConnection connection)
            : base(connection, false)
        {
        }

        public SampleDapperApplicationDbContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {
        }

        public virtual IDbSet<Product> Products { get; set; }
    }

    public class DapperDbContextConfiguration : DbConfiguration
    {
        public DapperDbContextConfiguration()
        {
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderServices("System.Data.SQLite", (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));
        }
    }
}
