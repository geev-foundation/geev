using System.Threading.Tasks;
using Geev.ZeroCore.SampleApp.Core;
using Shouldly;
using Xunit;

namespace Geev.Zero.Users
{
    public class UserManager_Options_Tests : GeevZeroTestBase
    {
        [Fact]
        public async Task InitializeOptionsAsync_Should_Not_Reset_Default_Options()
        {
            var userManager = LocalIocManager.Resolve<UserManager>();
            userManager.Options.Tokens.ProviderMap.Count.ShouldBeGreaterThan(0);

            await userManager.InitializeOptionsAsync(1);
            userManager.Options.Tokens.ProviderMap.Count.ShouldBeGreaterThan(0);
        }
    }
}