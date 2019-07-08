using Geev.Domain.Entities;
using Geev.NHibernate;
using Geev.NHibernate.Repositories;

namespace Geev.Zero.SampleApp.NHibernate.Repositories
{
    /// <summary>
    /// Base class for all repositories in this application
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimaryKey">Type of the primary key</typeparam>
    public abstract class GeevProjectNameRepositoryBase<TEntity, TPrimaryKey> : NhRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected GeevProjectNameRepositoryBase(ISessionProvider sessionProvider) : base(sessionProvider)
        {
        }

        //add common methods for all repositories
    }

    /// <summary>
    /// A shortcut of GeevProjectNameRepositoryBase for entities with integer Id.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class GeevProjectNameRepositoryBase<TEntity> : GeevProjectNameRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected GeevProjectNameRepositoryBase(ISessionProvider sessionProvider) : base(sessionProvider)
        {
        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
