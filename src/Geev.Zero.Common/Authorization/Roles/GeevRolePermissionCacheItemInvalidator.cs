using Geev.Dependency;
using Geev.Events.Bus.Entities;
using Geev.Events.Bus.Handlers;
using Geev.Runtime.Caching;

namespace Geev.Authorization.Roles
{
    public class GeevRolePermissionCacheItemInvalidator :
        IEventHandler<EntityChangedEventData<RolePermissionSetting>>,
        IEventHandler<EntityDeletedEventData<GeevRoleBase>>,
        ITransientDependency
    {
        private readonly ICacheManager _cacheManager;

        public GeevRolePermissionCacheItemInvalidator(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void HandleEvent(EntityChangedEventData<RolePermissionSetting> eventData)
        {
            var cacheKey = eventData.Entity.RoleId + "@" + (eventData.Entity.TenantId ?? 0);
            _cacheManager.GetRolePermissionCache().Remove(cacheKey);
        }

        public void HandleEvent(EntityDeletedEventData<GeevRoleBase> eventData)
        {
            var cacheKey = eventData.Entity.Id + "@" + (eventData.Entity.TenantId ?? 0);
            _cacheManager.GetRolePermissionCache().Remove(cacheKey);
        }
    }
}