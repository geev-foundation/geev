using System.Linq;
using System.Threading.Tasks;
using Geev.Authorization;
using Geev.Authorization.Users;
using Geev.Configuration;
using Geev.Configuration.Startup;
using Geev.Runtime.Session;
using Geev.Zero.Configuration;
using Geev.Zero.SampleApp.Authorization;
using Geev.Zero.SampleApp.MultiTenancy;
using Shouldly;
using Xunit;

namespace Geev.Zero.SampleApp.Tests.Users
{
    public class UserLogin_Tests : SampleAppTestBase
    {
        private readonly AppLogInManager _logInManager;

        public UserLogin_Tests()
        {
            UsingDbContext(UserLoginHelper.CreateTestUsers);
            _logInManager = LocalIocManager.Resolve<AppLogInManager>();
        }

        [Fact]
        public async Task Should_Login_With_Correct_Values_Without_MultiTenancy()
        {
            Resolve<IMultiTenancyConfig>().IsEnabled = false;
            GeevSession.TenantId = 1;

            var loginResult = await _logInManager.LoginAsync("user1", "123qwe");
            loginResult.Result.ShouldBe(GeevLoginResultType.Success);
            loginResult.User.Name.ShouldBe("User");
            loginResult.Identity.ShouldNotBe(null);

            UsingDbContext(context =>
            {
                context.UserLoginAttempts.Count().ShouldBe(1);
                context.UserLoginAttempts.FirstOrDefault(a => 
                    a.TenantId == GeevSession.TenantId &&
                    a.UserId == loginResult.User.Id &&
                    a.UserNameOrEmailAddress == "user1" &&
                    a.Result == GeevLoginResultType.Success
                    ).ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task Should_Not_Login_With_Invalid_UserName_Without_MultiTenancy()
        {
            Resolve<IMultiTenancyConfig>().IsEnabled = false;

            var loginResult = await _logInManager.LoginAsync("wrongUserName", "asdfgh");
            loginResult.Result.ShouldBe(GeevLoginResultType.InvalidUserNameOrEmailAddress);
            loginResult.User.ShouldBe(null);
            loginResult.Identity.ShouldBe(null);
            
            UsingDbContext(context =>
            {
                context.UserLoginAttempts.Count().ShouldBe(1);
                context.UserLoginAttempts.FirstOrDefault(a =>
                    a.UserNameOrEmailAddress == "wrongUserName" &&
                    a.Result == GeevLoginResultType.InvalidUserNameOrEmailAddress
                    ).ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task Should_Login_With_Correct_Values_With_MultiTenancy()
        {
            Resolve<IMultiTenancyConfig>().IsEnabled = true;
            GeevSession.TenantId = 1;

            var loginResult = await _logInManager.LoginAsync("user1", "123qwe", Tenant.DefaultTenantName);
            loginResult.Result.ShouldBe(GeevLoginResultType.Success);
            loginResult.User.Name.ShouldBe("User");
            loginResult.Identity.ShouldNotBe(null);
        }

        [Fact]
        public async Task Should_Not_Login_If_Email_Confirmation_Is_Enabled_And_User_Has_Not_Confirmed()
        {
            Resolve<IMultiTenancyConfig>().IsEnabled = true;

            //Set session
            GeevSession.TenantId = 1;
            GeevSession.UserId = 1;

            //Email confirmation is disabled as default
            (await _logInManager.LoginAsync("user1", "123qwe", Tenant.DefaultTenantName)).Result.ShouldBe(GeevLoginResultType.Success);

            //Change configuration
            await Resolve<ISettingManager>().ChangeSettingForTenantAsync(GeevSession.GetTenantId(), GeevZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin, "true");

            //Email confirmation is enabled now
            (await _logInManager.LoginAsync("user1", "123qwe", Tenant.DefaultTenantName)).Result.ShouldBe(GeevLoginResultType.UserEmailIsNotConfirmed);
        }

        [Fact]
        public async Task Should_Login_TenancyOwner_With_Correct_Values()
        {
            Resolve<IMultiTenancyConfig>().IsEnabled = true;

            var loginResult = await _logInManager.LoginAsync("userOwner", "123qwe");
            loginResult.Result.ShouldBe(GeevLoginResultType.Success);
            loginResult.User.Name.ShouldBe("Owner");
            loginResult.Identity.ShouldNotBe(null);
        }
    }
}
