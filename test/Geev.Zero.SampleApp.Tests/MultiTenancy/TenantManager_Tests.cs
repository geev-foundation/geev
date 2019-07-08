using System.Threading.Tasks;
using Geev.Zero.SampleApp.MultiTenancy;
using Shouldly;
using Xunit;

namespace Geev.Zero.SampleApp.Tests.MultiTenancy
{
    public class TenantManager_Tests : SampleAppTestBase
    {
        private readonly TenantManager _tenantManager;
        
        public TenantManager_Tests()
        {
            _tenantManager = Resolve<TenantManager>();
        }

        [Fact]
        public async Task Should_Not_Create_Duplicate_Tenant()
        {
            await _tenantManager.CreateAsync(new Tenant("Tenant-X", "Tenant X"));
            
            //Trying to re-create with same tenancy name

            await Assert.ThrowsAnyAsync<GeevException>(async () =>
            {
                await _tenantManager.CreateAsync(new Tenant("Tenant-X", "Tenant X"));
            });
        }
    }
}
