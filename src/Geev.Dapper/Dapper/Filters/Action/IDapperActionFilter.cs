using Geev.Dependency;
using Geev.Domain.Entities;

namespace Geev.Dapper.Filters.Action
{
    public interface IDapperActionFilter : ITransientDependency
    {
        void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>;
    }
}
