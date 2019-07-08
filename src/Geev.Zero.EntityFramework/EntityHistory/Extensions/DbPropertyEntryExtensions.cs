using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Geev.EntityHistory.Extensions
{
    internal static class DbPropertyEntryExtensions
    {
        internal static object GetNewValue(this DbPropertyEntry propertyEntry)
        {
            if (propertyEntry.EntityEntry.State == EntityState.Deleted)
            {
                return propertyEntry.OriginalValue;
            }

            return propertyEntry.CurrentValue;
        }

        internal static object GetOriginalValue(this DbPropertyEntry propertyEntry)
        {
            if (propertyEntry.EntityEntry.State == EntityState.Added)
            {
                return propertyEntry.CurrentValue;
            }

            return propertyEntry.OriginalValue;
        }

        internal static bool HasChanged(this DbPropertyEntry propertyEntry)
        {
            if (propertyEntry.EntityEntry.State == EntityState.Added)
            {
                return propertyEntry.CurrentValue != null;
            }

            if (propertyEntry.EntityEntry.State == EntityState.Deleted)
            {
                return propertyEntry.OriginalValue != null;
            }

            return !(propertyEntry.OriginalValue?.Equals(propertyEntry.CurrentValue)) ?? propertyEntry.CurrentValue == null;
        }
    }
}