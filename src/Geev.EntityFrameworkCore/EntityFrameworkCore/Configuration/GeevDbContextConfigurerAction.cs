using System;
using Microsoft.EntityFrameworkCore;

namespace Geev.EntityFrameworkCore.Configuration
{
    public class GeevDbContextConfigurerAction<TDbContext> : IGeevDbContextConfigurer<TDbContext>
        where TDbContext : DbContext
    {
        public Action<GeevDbContextConfiguration<TDbContext>> Action { get; set; }

        public GeevDbContextConfigurerAction(Action<GeevDbContextConfiguration<TDbContext>> action)
        {
            Action = action;
        }

        public void Configure(GeevDbContextConfiguration<TDbContext> configuration)
        {
            Action(configuration);
        }
    }
}