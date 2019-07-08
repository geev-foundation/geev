﻿using Geev.Domain.Entities;

namespace Geev.Dapper.Filters.Action
{
    public interface IDapperActionFilterExecuter
    {
        void ExecuteCreationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>;

        void ExecuteModificationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>;

        void ExecuteDeletionAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>;
    }
}
