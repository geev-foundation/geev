using System;
using Microsoft.EntityFrameworkCore;

namespace Geev.EntityFrameworkCore.Configuration
{
    public interface IGeevEfCoreConfiguration
    {
        void AddDbContext<TDbContext>(Action<GeevDbContextConfiguration<TDbContext>> action)
            where TDbContext : DbContext;
    }
}
