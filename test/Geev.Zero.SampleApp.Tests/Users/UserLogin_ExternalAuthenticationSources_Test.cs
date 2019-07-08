using System.Threading.Tasks;
using Geev.Authorization;
using Geev.Authorization.Users;
using Geev.Dependency;
using Geev.Modules;
using Geev.Zero.Configuration;
using Geev.Zero.SampleApp.Authorization;
using Geev.Zero.SampleApp.MultiTenancy;
using Geev.Zero.SampleApp.Users;
using Shouldly;
using Xunit;

namespace Geev.Zero.SampleApp.Tests.Users
{
    public class UserLogin_ExternalAuthenticationSources_Test : SampleAppTestBase<UserLogin_ExternalAuthenticationSources_Test.MyUserLoginTestModule>
    {
        private readonly AppLogInManager _logInManager;

        public UserLogin_ExternalAuthenticationSources_Test()
        {
            UsingDbContext(UserLoginHelper.CreateTestUsers);
            _logInManager = LocalIocManager.Resolve<AppLogInManager>();
        }

        [Fact]
        public async Task Should_Login_From_Fake_Authentication_Source()
        {
            var loginResult = await _logInManager.LoginAsync("fakeuser@mydomain.com", "123qwe", Tenant.DefaultTenantName);
            loginResult.Result.ShouldBe(GeevLoginResultType.Success);
            loginResult.User.AuthenticationSource.ShouldBe("FakeSource");
        }

        [Fact]
        public async Task Should_Fallback_To_Default_Login_Users()
        {
            var loginResult = await _logInManager.LoginAsync("owner@aspnetboilerplate.com", "123qwe");
            loginResult.Result.ShouldBe(GeevLoginResultType.Success);
        }

        [DependsOn(typeof(SampleAppTestModule))]
        public class MyUserLoginTestModule : GeevModule
        {
            public override void PreInitialize()
            {
                Configuration.Modules.Zero().UserManagement.ExternalAuthenticationSources.Add<FakeExternalAuthenticationSource>();
            }

            public override void Initialize()
            {
                IocManager.Register<FakeExternalAuthenticationSource>(DependencyLifeStyle.Transient);
            }
        }

        public class FakeExternalAuthenticationSource : DefaultExternalAuthenticationSource<Tenant, User>
        {
            public override string Name
            {
                get { return "FakeSource"; }
            }

            public override Task<bool> TryAuthenticateAsync(string userNameOrEmailAddress, string plainPassword, Tenant tenant)
            {
                return Task.FromResult(
                    userNameOrEmailAddress == "fakeuser@mydomain.com" &&
                    plainPassword == "123qwe" &&
                    tenant != null &&
                    tenant.TenancyName == Tenant.DefaultTenantName
                    );
            }

            public override Task<User> CreateUserAsync(string userNameOrEmailAddress, Tenant tenant)
            {
                var user = new User
                {
                    UserName = userNameOrEmailAddress,
                    Name = userNameOrEmailAddress,
                    Surname = userNameOrEmailAddress,
                    EmailAddress = userNameOrEmailAddress,
                    IsEmailConfirmed = true,
                    IsActive = true
                };

                user.SetNormalizedNames();

                return Task.FromResult(user);
            }
        }
    }
}