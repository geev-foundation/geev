using System;
using Geev.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Geev.EntityFrameworkCore
{
    public static class GeevEfCoreServiceCollectionExtensions
    {
        public static void AddGeevDbContext<TDbContext>(
            this IServiceCollection services,
            Action<GeevDbContextConfiguration<TDbContext>> action)
            where TDbContext : DbContext
        {
            services.AddSingleton(
                typeof(IGeevDbContextConfigurer<TDbContext>),
                new GeevDbContextConfigurerAction<TDbContext>(action)
            );
        }
    }
}
