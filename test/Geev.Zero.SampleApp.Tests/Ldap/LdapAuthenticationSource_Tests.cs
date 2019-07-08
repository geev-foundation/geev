using System;
using System.Threading.Tasks;
using Geev.Authorization;
using Geev.Authorization.Users;
using Geev.Configuration;
using Geev.Dependency;
using Geev.Modules;
using Geev.Zero.Ldap;
using Geev.Zero.Ldap.Authentication;
using Geev.Zero.Ldap.Configuration;
using Geev.Zero.SampleApp.Authorization;
using Geev.Zero.SampleApp.MultiTenancy;
using Geev.Zero.SampleApp.Users;
using Shouldly;
using Xunit;

namespace Geev.Zero.SampleApp.Tests.Ldap
{
    public class LdapAuthenticationSource_Tests : SampleAppTestBase<LdapAuthenticationSource_Tests.MyUserLoginTestModule>
    {
        private readonly AppLogInManager _logInManager;

        public LdapAuthenticationSource_Tests()
        {
            _logInManager = Resolve<AppLogInManager>();
        }

        //[Fact]
        public async Task Should_Login_From_Ldap_Without_Any_Configuration()
        {
            var result = await _logInManager.LoginAsync("-","-", Tenant.DefaultTenantName);
            result.Result.ShouldBe(GeevLoginResultType.Success);
        }

        //[Fact]
        public async Task Should_Not_Login_From_Ldap_If_Disabled()
        {
            var settingManager = Resolve<ISettingManager>();
            var defaultTenant = GetDefaultTenant();

            await settingManager.ChangeSettingForTenantAsync(defaultTenant.Id, LdapSettingNames.IsEnabled, "false");

            var result = await _logInManager.LoginAsync("-", "-", Tenant.DefaultTenantName);
            result.Result.ShouldBe(GeevLoginResultType.InvalidUserNameOrEmailAddress);
        }

        //[Fact]
        public async Task Should_Login_From_Ldap_With_Properly_Configured()
        {
            var settingManager = Resolve<ISettingManager>();
            var defaultTenant = GetDefaultTenant();

            await settingManager.ChangeSettingForTenantAsync(defaultTenant.Id, LdapSettingNames.Domain, "-");
            await settingManager.ChangeSettingForTenantAsync(defaultTenant.Id, LdapSettingNames.UserName, "-");
            await settingManager.ChangeSettingForTenantAsync(defaultTenant.Id, LdapSettingNames.Password, "-");

            var result = await _logInManager.LoginAsync("-", "-", Tenant.DefaultTenantName);
            result.Result.ShouldBe(GeevLoginResultType.Success);
        }

        //[Fact]
        public async Task Should_Not_Login_From_Ldap_With_Wrong_Configuration()
        {
            var settingManager = Resolve<ISettingManager>();
            var defaultTenant = GetDefaultTenant();

            await settingManager.ChangeSettingForTenantAsync(defaultTenant.Id, LdapSettingNames.Domain, "InvalidDomain");
            await settingManager.ChangeSettingForTenantAsync(defaultTenant.Id, LdapSettingNames.UserName, "NoUserName");
            await settingManager.ChangeSettingForTenantAsync(defaultTenant.Id, LdapSettingNames.Password, "123123123123");

            await Assert.ThrowsAnyAsync<Exception>(() => _logInManager.LoginAsync("testuser", "testpass", Tenant.DefaultTenantName));
        }

        [DependsOn(typeof(GeevZeroLdapModule), typeof(SampleAppTestModule))]
        public class MyUserLoginTestModule : GeevModule
        {
            public override void PreInitialize()
            {
                Configuration.Modules.ZeroLdap().Enable(typeof (MyLdapAuthenticationSource));
            }

            public override void Initialize()
            {
                //This is needed just for this test, not for real apps
                IocManager.Register<MyLdapAuthenticationSource>(DependencyLifeStyle.Transient);
            }
        }

        public class MyLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
        {
            public MyLdapAuthenticationSource(ILdapSettings settings, IGeevZeroLdapModuleConfig ldapModuleConfig)
                : base(settings, ldapModuleConfig)
            {

            }
        }
    }
}
