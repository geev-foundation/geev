using Geev.Authorization.Users;
using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.Events.Bus.Entities;
using Geev.Events.Bus.Handlers;
using Geev.Runtime.Caching;
using Geev.Runtime.Security;

namespace Geev.MultiTenancy
{
    public class TenantCache<TTenant, TUser> : ITenantCache, IEventHandler<EntityChangedEventData<TTenant>>
        where TTenant : GeevTenant<TUser>
        where TUser : GeevUserBase
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<TTenant> _tenantRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public TenantCache(
            ICacheManager cacheManager,
            IRepository<TTenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _cacheManager = cacheManager;
            _tenantRepository = tenantRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public virtual TenantCacheItem Get(int tenantId)
        {
            var cacheItem = GetOrNull(tenantId);

            if (cacheItem == null)
            {
                throw new GeevException("There is no tenant with given id: " + tenantId);
            }

            return cacheItem;
        }

        public virtual TenantCacheItem Get(string tenancyName)
        {
            var cacheItem = GetOrNull(tenancyName);

            if (cacheItem == null)
            {
                throw new GeevException("There is no tenant with given tenancy name: " + tenancyName);
            }

            return cacheItem;
        }

        public virtual TenantCacheItem GetOrNull(string tenancyName)
        {
            var tenantId = _cacheManager
                .GetTenantByNameCache()
                .Get(
                    tenancyName.ToLowerInvariant(),
                    () => GetTenantOrNull(tenancyName)?.Id
                );

            if (tenantId == null)
            {
                return null;
            }

            return Get(tenantId.Value);
        }

        public TenantCacheItem GetOrNull(int tenantId)
        {
            return _cacheManager
                .GetTenantCache()
                .Get(
                    tenantId,
                    () =>
                    {
                        var tenant = GetTenantOrNull(tenantId);
                        if (tenant == null)
                        {
                            return null;
                        }

                        return CreateTenantCacheItem(tenant);
                    }
                );
        }

        protected virtual TenantCacheItem CreateTenantCacheItem(TTenant tenant)
        {
            return new TenantCacheItem
            {
                Id = tenant.Id,
                Name = tenant.Name,
                TenancyName = tenant.TenancyName,
                EditionId = tenant.EditionId,
                ConnectionString = SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString),
                IsActive = tenant.IsActive
            };
        }

        [UnitOfWork]
        protected virtual TTenant GetTenantOrNull(int tenantId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                return _tenantRepository.FirstOrDefault(tenantId);
            }
        }

        [UnitOfWork]
        protected virtual TTenant GetTenantOrNull(string tenancyName)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                return _tenantRepository.FirstOrDefault(t => t.TenancyName == tenancyName);
            }
        }

        public void HandleEvent(EntityChangedEventData<TTenant> eventData)
        {
            var existingCacheItem = _cacheManager.GetTenantCache().GetOrDefault(eventData.Entity.Id);

            _cacheManager
                .GetTenantByNameCache()
                .Remove(
                    existingCacheItem != null
                        ? existingCacheItem.TenancyName.ToLowerInvariant()
                        : eventData.Entity.TenancyName.ToLowerInvariant()
                );

            _cacheManager
                .GetTenantCache()
                .Remove(eventData.Entity.Id);
        }
    }
}