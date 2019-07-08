using Geev.Application.Features;
using Geev.Domain.Repositories;
using Geev.MultiTenancy;
using Geev.Zero.SampleApp.Editions;
using Geev.Zero.SampleApp.Users;

namespace Geev.Zero.SampleApp.MultiTenancy
{
    public class TenantManager : GeevTenantManager<Tenant, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository,
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
            EditionManager editionManager,
            IGeevZeroFeatureValueStore featureValueStore) :
            base(tenantRepository, tenantFeatureRepository, editionManager, featureValueStore)
        {
        }
    }
}
