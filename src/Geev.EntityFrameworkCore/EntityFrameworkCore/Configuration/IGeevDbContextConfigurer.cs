using Microsoft.EntityFrameworkCore;

namespace Geev.EntityFrameworkCore.Configuration
{
    public interface IGeevDbContextConfigurer<TDbContext>
        where TDbContext : DbContext
    {
        void Configure(GeevDbContextConfiguration<TDbContext> configuration);
    }
}