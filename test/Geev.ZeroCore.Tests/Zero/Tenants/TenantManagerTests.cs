using System.Linq;
using System.Threading.Tasks;
using Geev.Domain.Uow;
using Geev.ZeroCore.SampleApp.Application;
using Geev.ZeroCore.SampleApp.Core;
using Shouldly;
using Xunit;

namespace Geev.Zero.Tenants
{
    public class TenantManagerTests : GeevZeroTestBase
    {
        private readonly TenantManager _tenantManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public TenantManagerTests()
        {
            _tenantManager = Resolve<TenantManager>();
            _unitOfWorkManager = Resolve<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task Should_Not_Insert_Duplicate_Features()
        {
            const int tenantId = 1;

            UsingDbContext(tenantId, (context) =>
            {
                context.FeatureSettings.Count(f => f.TenantId == tenantId).ShouldBe(0);
            });

            await ChangeTenantFeatureValueAsync(tenantId, AppFeatures.SimpleIntFeature, "1");

            UsingDbContext(tenantId, (context) =>
            {
                context.FeatureSettings.Count(f => f.TenantId == tenantId).ShouldBe(1);
            });

            await ChangeTenantFeatureValueAsync(tenantId, AppFeatures.SimpleIntFeature, "2");

            UsingDbContext(tenantId, (context) =>
            {
                context.FeatureSettings.Count(f => f.TenantId == tenantId).ShouldBe(1);
            });

            await ChangeTenantFeatureValueAsync(tenantId, AppFeatures.SimpleIntFeature, "0");

            UsingDbContext(tenantId, (context) =>
            {
                context.FeatureSettings.Count(f => f.TenantId == tenantId).ShouldBe(0);
            });
        }

        [Fact]
        public async Task Should_Reset_Tenant_Features()
        {
            const int tenantId = 1;

            UsingDbContext(tenantId, (context) =>
            {
                context.FeatureSettings.Count(f => f.TenantId == tenantId).ShouldBe(0);
            });

            await ChangeTenantFeatureValueAsync(tenantId, AppFeatures.SimpleIntFeature, "1");

            UsingDbContext(tenantId, (context) =>
            {
                context.FeatureSettings.Count(f => f.TenantId == tenantId).ShouldBe(1);
            });

            using (var uow = _unitOfWorkManager.Begin())
            {
                using (_unitOfWorkManager.Current.SetTenantId(null))
                {
                    await _tenantManager.ResetAllFeaturesAsync(tenantId);
                }

                await uow.CompleteAsync();
            }

            UsingDbContext(tenantId, (context) =>
            {
                context.FeatureSettings.Count(f => f.TenantId == tenantId).ShouldBe(0);
            });
        }

        private async Task ChangeTenantFeatureValueAsync(int tenantId, string name, string value)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (_unitOfWorkManager.Current.SetTenantId(null))
                {
                    await _tenantManager.SetFeatureValueAsync(tenantId, name, value);
                }

                await uow.CompleteAsync();
            }
        }
    }
}
