using System.Threading.Tasks;
using Geev.TestBase;
using Geev.Web.Configuration;
using Shouldly;
using Xunit;

namespace Geev.Web.Common.Tests.Configuration
{
    public class GeevUserConfigurationBuilder_Tests : GeevIntegratedTestBase<GeevWebCommonTestModule>
    {
        private readonly GeevUserConfigurationBuilder _geevUserConfigurationBuilder;

        public GeevUserConfigurationBuilder_Tests()
        {
            _geevUserConfigurationBuilder = Resolve<GeevUserConfigurationBuilder>();
        }

        [Fact]
        public async Task GeevUserConfigurationBuilder_Should_Build_User_Configuration()
        {
            var userConfiguration = await _geevUserConfigurationBuilder.GetAll();
            userConfiguration.ShouldNotBe(null);

            userConfiguration.MultiTenancy.ShouldNotBe(null);
            userConfiguration.Session.ShouldNotBe(null);
            userConfiguration.Localization.ShouldNotBe(null);
            userConfiguration.Features.ShouldNotBe(null);
            userConfiguration.Auth.ShouldNotBe(null);
            userConfiguration.Nav.ShouldNotBe(null);
            userConfiguration.Setting.ShouldNotBe(null);
            userConfiguration.Clock.ShouldNotBe(null);
            userConfiguration.Timing.ShouldNotBe(null);
            userConfiguration.Security.ShouldNotBe(null);
            userConfiguration.Custom.ShouldNotBe(null);
        }

        [Fact]
        public async Task GeevUserConfigurationBuilder_Setting_Which_RequiresAuthentication()
        {
            var userConfiguration = await _geevUserConfigurationBuilder.GetAll();
            userConfiguration.Setting.Values.ShouldNotContain(s => s.Key == "GeevWebCommonTestModule.Test.Setting1");
        }

        [Fact]
        public async Task GeevUserConfigurationBuilder_Setting_Which_RequiresPermission()
        {
            var userConfiguration = await _geevUserConfigurationBuilder.GetAll();
            userConfiguration.Setting.Values.ShouldNotContain(s => s.Key == "GeevWebCommonTestModule.Test.Setting2");
        }

        [Fact]
        public async Task GeevUserConfigurationBuilder_Setting_Which_RequiresAuthentication_For_Authanticated_User()
        {
            LoginAsDefaultTenantAdmin();

            var userConfiguration = await _geevUserConfigurationBuilder.GetAll();
            userConfiguration.Setting.Values.ShouldContain(s => s.Key == "GeevWebCommonTestModule.Test.Setting1");
        }

        [Fact]
        public async Task GeevUserConfigurationBuilder_Setting_Which_RequiresPermission_For_Authorized_User()
        {
            LoginAsDefaultTenantAdmin();

            var userConfiguration = await _geevUserConfigurationBuilder.GetAll();
            userConfiguration.Setting.Values.ShouldContain(s => s.Key == "GeevWebCommonTestModule.Test.Setting2");
        }

        private void LoginAsDefaultTenantAdmin()
        {
            GeevSession.UserId = 2;
            GeevSession.TenantId = 1;
        }
    }
}
