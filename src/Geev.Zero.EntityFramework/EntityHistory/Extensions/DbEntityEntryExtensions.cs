using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using Geev.Auditing;
using Geev.Domain.Entities;
using Geev.Extensions;

namespace Geev.EntityHistory.Extensions
{
    internal static class DbEntityEntryExtensions
    {
        internal static Type GetEntityBaseType(this DbEntityEntry entityEntry)
        {
            return ObjectContext.GetObjectType(entityEntry.Entity.GetType());
        }

        internal static PropertyInfo GetPropertyInfo(this DbEntityEntry entityEntry, string propertyName)
        {
            return entityEntry.GetEntityBaseType().GetProperty(propertyName);
        }

        internal static DbPropertyValues GetPropertyValues(this DbEntityEntry entityEntry)
        {
            if (entityEntry.State == EntityState.Deleted)
            {
                return entityEntry.OriginalValues;
            }
            return entityEntry.CurrentValues;
        }

        internal static bool HasAuditedProperties(this DbEntityEntry entityEntry)
        {
            var propertyNames = entityEntry.GetPropertyValues().PropertyNames;
            var entityType = entityEntry.GetEntityBaseType();
            return propertyNames.Any(p => entityType.GetProperty(p)?.IsDefined(typeof(AuditedAttribute)) ?? false);
        }

        internal static bool IsCreated(this DbEntityEntry entityEntry)
        {
            return entityEntry.State == EntityState.Added;
        }

        internal static bool IsDeleted(this DbEntityEntry entityEntry)
        {
            if (entityEntry.State == EntityState.Deleted)
            {
                return true;
            }
            var entity = entityEntry.Entity;
            return entity is ISoftDelete && entity.As<ISoftDelete>().IsDeleted;
        }
    }
}