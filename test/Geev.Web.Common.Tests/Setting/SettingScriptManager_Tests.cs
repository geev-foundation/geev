using System;
using System.Threading.Tasks;
using Geev.Configuration;
using Geev.Runtime.Session;
using Geev.TestBase;
using Geev.Web.Http;
using Geev.Web.Settings;
using Shouldly;
using Xunit;

namespace Geev.Web.Common.Tests.Setting
{
    public class SettingScriptManager_Tests : GeevIntegratedTestBase<GeevWebCommonTestModule>
    {
        private readonly ISettingScriptManager _settingScriptManager;
        private readonly ISettingManager _settingManager;

        public SettingScriptManager_Tests()
        {
            _settingScriptManager = Resolve<ISettingScriptManager>();
            _settingManager = Resolve<ISettingManager>();
        }

        [Fact]
        public async Task SettingScriptManager_Setting_Which_RequiresAuthentication()
        {
            var scripts = await _settingScriptManager.GetScriptAsync();
            scripts.ShouldNotContain("GeevWebCommonTestModule.Test.Setting1");
        }

        [Fact]
        public async Task SettingScriptManager_Setting_Which_RequiresPermission()
        {
            var scripts = await _settingScriptManager.GetScriptAsync();
            scripts.ShouldNotContain("GeevWebCommonTestModule.Test.Setting2");
        }

        [Fact]
        public async Task SettingScriptManager_Setting_Which_RequiresPermission_For_Authanticated_User()
        {
            LoginAsDefaultTenantAdmin();

            var scripts = await _settingScriptManager.GetScriptAsync();
            scripts.ShouldContain("GeevWebCommonTestModule.Test.Setting1");
        }

        [Fact]
        public async Task SettingScriptManager_Setting_Which_RequiresPermission_For_Authorized_User()
        {
            LoginAsDefaultTenantAdmin();

            var scripts = await _settingScriptManager.GetScriptAsync();
            scripts.ShouldContain("GeevWebCommonTestModule.Test.Setting2");
        }

        [Fact]
        public async Task SettingScriptManager_Encode_Test()
        {
            LoginAsDefaultTenantAdmin();

            var scripts = await _settingScriptManager.GetScriptAsync();
            scripts.ShouldContain("GeevWebCommonTestModule.Test.Setting3");
            scripts.ShouldContain("Test \\u003e Value3");
        }

        private void LoginAsDefaultTenantAdmin()
        {
            GeevSession.UserId = 2;
            GeevSession.TenantId = 1;
        }
    }
}
