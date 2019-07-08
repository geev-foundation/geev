using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Geev.ZeroCore.SampleApp.Core
{
    public static class ServicesCollectionDependencyRegistrar
    {
        public static void Register(ServiceCollection services)
        {
            services.AddLogging();

            services.AddGeevIdentity<Tenant, User, Role>()
                .AddGeevTenantManager<TenantManager>()
                .AddGeevEditionManager<EditionManager>()
                .AddGeevRoleManager<RoleManager>()
                .AddGeevUserManager<UserManager>()
                .AddGeevSignInManager<SignInManager>()
                .AddGeevLogInManager<LogInManager>()
                .AddGeevUserClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
                .AddGeevSecurityStampValidator<SecurityStampValidator>()
                .AddPermissionChecker<PermissionChecker>()
                .AddGeevUserStore<UserStore>()
                .AddGeevRoleStore<RoleStore>()
                .AddFeatureValueStore<FeatureValueStore>()
                .AddDefaultTokenProviders();
        }
    }
}
