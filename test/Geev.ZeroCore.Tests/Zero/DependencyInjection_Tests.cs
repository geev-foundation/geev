using Geev.Application.Editions;
using Geev.Application.Features;
using Geev.Authorization;
using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.MultiTenancy;
using Geev.ZeroCore.SampleApp.Core;
using Microsoft.AspNetCore.Identity;
using Xunit;

using SecurityStampValidator = Geev.ZeroCore.SampleApp.Core.SecurityStampValidator;

namespace Geev.Zero
{
    public class DependencyInjection_Tests : GeevZeroTestBase
    {
        [Fact]
        public void Should_Resolve_UserManager()
        {
            LocalIocManager.Resolve<UserManager<User>>();
            LocalIocManager.Resolve<GeevUserManager<Role, User>>();
            LocalIocManager.Resolve<UserManager>();
        }

        [Fact]
        public void Should_Resolve_RoleManager()
        {
            LocalIocManager.Resolve<RoleManager<Role>>();
            LocalIocManager.Resolve<GeevRoleManager<Role, User>>();
            LocalIocManager.Resolve<RoleManager>();
        }

        [Fact]
        public void Should_Resolve_SignInManager()
        {
            LocalIocManager.Resolve<SignInManager<User>>();
            LocalIocManager.Resolve<GeevSignInManager<Tenant, Role, User>>();
            LocalIocManager.Resolve<SignInManager>();
        }

        [Fact]
        public void Should_Resolve_LoginManager()
        {
            LocalIocManager.Resolve<GeevLogInManager<Tenant, Role, User>>();
            LocalIocManager.Resolve<LogInManager>();
        }

        [Fact]
        public void Should_Resolve_SecurityStampValidator()
        {
            LocalIocManager.Resolve<GeevSecurityStampValidator<Tenant, Role, User>>();
            LocalIocManager.Resolve<SecurityStampValidator<User>>();
            LocalIocManager.Resolve<SecurityStampValidator>();
        }

        [Fact]
        public void Should_Resolve_UserClaimsPrincipalFactory()
        {
            LocalIocManager.Resolve<UserClaimsPrincipalFactory<User, Role>>();
            LocalIocManager.Resolve<GeevUserClaimsPrincipalFactory<User, Role>>();
            LocalIocManager.Resolve<IUserClaimsPrincipalFactory<User>>();
            LocalIocManager.Resolve<UserClaimsPrincipalFactory>();
        }

        [Fact]
        public void Should_Resolve_TenantManager()
        {
            LocalIocManager.Resolve<GeevTenantManager<Tenant, User>>();
            LocalIocManager.Resolve<TenantManager>();
        }

        [Fact]
        public void Should_Resolve_EditionManager()
        {
            LocalIocManager.Resolve<GeevEditionManager>();
            LocalIocManager.Resolve<EditionManager>();
        }

        [Fact]
        public void Should_Resolve_PermissionChecker()
        {
            LocalIocManager.Resolve<IPermissionChecker>();
            LocalIocManager.Resolve<PermissionChecker<Role, User>>();
            LocalIocManager.Resolve<PermissionChecker>();
        }

        [Fact]
        public void Should_Resolve_FeatureValueStore()
        {
            LocalIocManager.Resolve<IFeatureValueStore>();
            LocalIocManager.Resolve<GeevFeatureValueStore<Tenant, User>>();
            LocalIocManager.Resolve<FeatureValueStore>();
        }

        [Fact]
        public void Should_Resolve_UserStore()
        {
            LocalIocManager.Resolve<IUserStore<User>>();
            LocalIocManager.Resolve<GeevUserStore<Role, User>>();
            LocalIocManager.Resolve<UserStore>();
        }

        [Fact]
        public void Should_Resolve_RoleStore()
        {
            LocalIocManager.Resolve<IRoleStore<Role>>();
            LocalIocManager.Resolve<GeevRoleStore<Role, User>>();
            LocalIocManager.Resolve<RoleStore>();
        }
    }
}
