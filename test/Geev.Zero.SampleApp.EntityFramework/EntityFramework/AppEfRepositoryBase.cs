using Geev.Domain.Entities;
using Geev.Domain.Repositories;
using Geev.EntityFramework;
using Geev.EntityFramework.Repositories;

namespace Geev.Zero.SampleApp.EntityFramework
{
    public class AppEfRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<AppDbContext, TEntity, TPrimaryKey>
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