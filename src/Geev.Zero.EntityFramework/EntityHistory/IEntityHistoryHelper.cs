using System.Data.Entity;
using System.Threading.Tasks;
using Geev.Threading;

namespace Geev.EntityHistory
{
    public interface IEntityHistoryHelper
    {
        EntityChangeSet CreateEntityChangeSet(DbContext context);

        Task SaveAsync(DbContext context, EntityChangeSet changeSet);
    }

    public static class EntityHistoryHelperExtensions
    {
        public static void Save(this IEntityHistoryHelper entityHistoryHelper, DbContext context, EntityChangeSet changeSet)
        {
            AsyncHelper.RunSync(() => entityHistoryHelper.SaveAsync(context, changeSet));
        }
    }
}
