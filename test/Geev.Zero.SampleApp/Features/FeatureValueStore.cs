using Geev.Application.Features;
using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.MultiTenancy;
using Geev.Runtime.Caching;
using Geev.Zero.SampleApp.MultiTenancy;
using Geev.Zero.SampleApp.Users;

namespace Geev.Zero.SampleApp.Features
{
    public class FeatureValueStore : GeevFeatureValueStore<Tenant, User>
    {
        public FeatureValueStore(ICacheManager cacheManager,
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
            IRepository<Tenant> tenantRepository,
            IRepository<EditionFeatureSetting, long> editionFeatureRepository,
            IFeatureManager featureManager,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                  cacheManager, 
                  tenantFeatureRepository, 
                  tenantRepository, 
                  editionFeatureRepository, 
                  featureManager,
                  unitOfWorkManager)
        {

        }
    }
}