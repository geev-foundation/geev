using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Geev.EntityFrameworkCore.Configuration
{
    public class GeevDbContextConfiguration<TDbContext>
        where TDbContext : DbContext
    {
        public string ConnectionString {get; internal set; }

        public DbConnection ExistingConnection { get; internal set; }

        public DbContextOptionsBuilder<TDbContext> DbContextOptions { get; }
        
        public GeevDbContextConfiguration(string connectionString, DbConnection existingConnection)
        {
            ConnectionString = connectionString;
            ExistingConnection = existingConnection;

            DbContextOptions = new DbContextOptionsBuilder<TDbContext>();
        }
    }
}