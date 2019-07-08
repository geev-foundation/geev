using Microsoft.EntityFrameworkCore;

namespace Geev.ZeroCore.SampleApp.EntityFramework
{
    public static class GeevZeroTemplateDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<SampleAppDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }
    }
}