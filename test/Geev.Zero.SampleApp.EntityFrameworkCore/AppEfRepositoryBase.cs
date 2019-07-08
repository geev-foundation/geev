using Geev.Domain.Entities;
using Geev.Domain.Repositories;
using Geev.EntityFrameworkCore;
using Geev.EntityFrameworkCore.Repositories;

namespace Geev.Zero.SampleApp.EntityFrameworkCore
{
    public class AppEfRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<AppDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public AppEfRepositoryBase(IDbContextProvider<AppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }

    public class AppEfRepositoryBase<TEntity> : AppEfRepositoryBase<TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        public AppEfRepositoryBase(IDbContextProvider<AppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}