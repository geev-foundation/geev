using System.Threading.Tasks;
using Geev.Application.Editions;
using Geev.Authorization.Users;
using Geev.Collections.Extensions;
using Geev.Dependency;
using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.Events.Bus.Entities;
using Geev.Events.Bus.Handlers;
using Geev.MultiTenancy;
using Geev.Runtime.Caching;

namespace Geev.Application.Features
{
    /// <summary>
    /// Implements <see cref="IFeatureValueStore"/>.
    /// </summary>
    public class GeevFeatureValueStore<TTenant, TUser> :
        IGeevZeroFeatureValueStore,
        ITransientDependency,
        IEventHandler<EntityChangedEventData<Edition>>,
        IEventHandler<EntityChangedEventData<EditionFeatureSetting>>

        where TTenant : GeevTenant<TUser>
        where TUser : GeevUserBase
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<TenantFeatureSetting, long> _tenantFeatureRepository;
        private readonly IRepository<TTenant> _tenantRepository;
        private readonly IRepository<EditionFeatureSetting, long> _editionFeatureRepository;
        private readonly IFeatureManager _featureManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeevFeatureValueStore{TTenant, TUser}"/> class.
        /// </summary>
        public GeevFeatureValueStore(
            ICacheManager cacheManager,
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
            IRepository<TTenant> tenantRepository,
            IRepository<EditionFeatureSetting, long> editionFeatureRepository,
            IFeatureManager featureManager,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _cacheManager = cacheManager;
            _tenantFeatureRepository = tenantFeatureRepository;
            _tenantRepository = tenantRepository;
            _editionFeatureRepository = editionFeatureRepository;
            _featureManager = featureManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <inheritdoc/>
        public virtual Task<string> GetValueOrNullAsync(int tenantId, Feature feature)
        {
            return GetValueOrNullAsync(tenantId, feature.Name);
        }

        public virtual async Task<string> GetEditionValueOrNullAsync(int editionId, string featureName)
        {
            var cacheItem = await GetEditionFeatureCacheItemAsync(editionId);
            return cacheItem.FeatureValues.GetOrDefault(featureName);
        }

        public virtual async Task<string> GetValueOrNullAsync(int tenantId, string featureName)
        {
            var cacheItem = await GetTenantFeatureCacheItemAsync(tenantId);
            var value = cacheItem.FeatureValues.GetOrDefault(featureName);
            if (value != null)
            {
                return value;
            }

            if (cacheItem.EditionId.HasValue)
            {
                value = await GetEditionValueOrNullAsync(cacheItem.EditionId.Value, featureName);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }

        [UnitOfWork]
        public virtual async Task SetEditionFeatureValueAsync(int editionId, string featureName, string value)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                if (await GetEditionValueOrNullAsync(editionId, featureName) == value)
                {
                    return;
                }

                var currentFeature = await _editionFeatureRepository.FirstOrDefaultAsync(f => f.EditionId == editionId && f.Name == featureName);

                var feature = _featureManager.GetOrNull(featureName);
                if (feature == null || feature.DefaultValue == value)
                {
                    if (currentFeature != null)
                    {
                        await _editionFeatureRepository.DeleteAsync(currentFeature);
                    }

                    return;
                }

                if (currentFeature == null)
                {
                    await _editionFeatureRepository.InsertAsync(new EditionFeatureSetting(editionId, featureName, value));
                }
                else
                {
                    currentFeature.Value = value;
                }
            }
        }

        protected virtual async Task<TenantFeatureCacheItem> GetTenantFeatureCacheItemAsync(int tenantId)
        {
            return await _cacheManager.GetTenantFeatureCache().GetAsync(tenantId, async () =>
            {
                TTenant tenant;
                using (var uow = _unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(null))
                    {
                        tenant = await _tenantRepository.GetAsync(tenantId);

                        await uow.CompleteAsync();
                    }
                }

                var newCacheItem = new TenantFeatureCacheItem { EditionId = tenant.EditionId };

                using (var uow = _unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var featureSettings = await _tenantFeatureRepository.GetAllListAsync();
                        foreach (var featureSetting in featureSettings)
                        {
                            newCacheItem.FeatureValues[featureSetting.Name] = featureSetting.Value;
                        }

                        await uow.CompleteAsync();
                    }
                }

                return newCacheItem;
            });
        }

        protected virtual async Task<EditionfeatureCacheItem> GetEditionFeatureCacheItemAsync(int editionId)
        {
            return await _cacheManager
                .GetEditionFeatureCache()
                .GetAsync(
                    editionId,
                    async () => await CreateEditionFeatureCacheItem(editionId)
                );
        }

        protected virtual async Task<EditionfeatureCacheItem> CreateEditionFeatureCacheItem(int editionId)
        {
            var newCacheItem = new EditionfeatureCacheItem();

            using (var uow = _unitOfWorkManager.Begin())
            {
                using (_unitOfWorkManager.Current.SetTenantId(null))
                {
                    var featureSettings = await _editionFeatureRepository.GetAllListAsync(f => f.EditionId == editionId);
                    foreach (var featureSetting in featureSettings)
                    {
                        newCacheItem.FeatureValues[featureSetting.Name] = featureSetting.Value;
                    }

                    await uow.CompleteAsync();
                }
            }
            
            return newCacheItem;
        }

        public virtual void HandleEvent(EntityChangedEventData<EditionFeatureSetting> eventData)
        {
            _cacheManager.GetEditionFeatureCache().Remove(eventData.Entity.EditionId);
        }

        public virtual void HandleEvent(EntityChangedEventData<Edition> eventData)
        {
            if (eventData.Entity.IsTransient())
            {
                return;
            }

            _cacheManager.GetEditionFeatureCache().Remove(eventData.Entity.Id);
        }
    }
}