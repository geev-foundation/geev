using System;
using Geev.Application.Editions;
using Geev.Application.Features;
using Geev.Authorization;
using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.MultiTenancy;
using Geev.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace - This is done to add extension methods to Microsoft.Extensions.DependencyInjection namespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class GeevZeroServiceCollectionExtensions
    {
        public static GeevIdentityBuilder AddGeevIdentity<TTenant, TUser, TRole>(this IServiceCollection services)
            where TTenant : GeevTenant<TUser>
            where TRole : GeevRole<TUser>, new()
            where TUser : GeevUser<TUser>
        {
            return services.AddGeevIdentity<TTenant, TUser, TRole>(setupAction: null);
        }

        public static GeevIdentityBuilder AddGeevIdentity<TTenant, TUser, TRole>(this IServiceCollection services, Action<IdentityOptions> setupAction)
            where TTenant : GeevTenant<TUser>
            where TRole : GeevRole<TUser>, new()
            where TUser : GeevUser<TUser>
        {
            services.AddSingleton<IGeevZeroEntityTypes>(new GeevZeroEntityTypes
            {
                Tenant = typeof(TTenant),
                Role = typeof(TRole),
                User = typeof(TUser)
            });

            //GeevTenantManager
            services.TryAddScoped<GeevTenantManager<TTenant, TUser>>();

            //GeevEditionManager
            services.TryAddScoped<GeevEditionManager>();

            //GeevRoleManager
            services.TryAddScoped<GeevRoleManager<TRole, TUser>>();
            services.TryAddScoped(typeof(RoleManager<TRole>), provider => provider.GetService(typeof(GeevRoleManager<TRole, TUser>)));

            //GeevUserManager
            services.TryAddScoped<GeevUserManager<TRole, TUser>>();
            services.TryAddScoped(typeof(UserManager<TUser>), provider => provider.GetService(typeof(GeevUserManager<TRole, TUser>)));

            //SignInManager
            services.TryAddScoped<GeevSignInManager<TTenant, TRole, TUser>>();
            services.TryAddScoped(typeof(SignInManager<TUser>), provider => provider.GetService(typeof(GeevSignInManager<TTenant, TRole, TUser>)));

            //GeevLogInManager
            services.TryAddScoped<GeevLogInManager<TTenant, TRole, TUser>>();

            //GeevUserClaimsPrincipalFactory
            services.TryAddScoped<GeevUserClaimsPrincipalFactory<TUser, TRole>>();
            services.TryAddScoped(typeof(UserClaimsPrincipalFactory<TUser, TRole>), provider => provider.GetService(typeof(GeevUserClaimsPrincipalFactory<TUser, TRole>)));
            services.TryAddScoped(typeof(IUserClaimsPrincipalFactory<TUser>), provider => provider.GetService(typeof(GeevUserClaimsPrincipalFactory<TUser, TRole>)));

            //GeevSecurityStampValidator
            services.TryAddScoped<GeevSecurityStampValidator<TTenant, TRole, TUser>>();
            services.TryAddScoped(typeof(SecurityStampValidator<TUser>), provider => provider.GetService(typeof(GeevSecurityStampValidator<TTenant, TRole, TUser>)));
            services.TryAddScoped(typeof(ISecurityStampValidator), provider => provider.GetService(typeof(GeevSecurityStampValidator<TTenant, TRole, TUser>)));

            //PermissionChecker
            services.TryAddScoped<PermissionChecker<TRole, TUser>>();
            services.TryAddScoped(typeof(IPermissionChecker), provider => provider.GetService(typeof(PermissionChecker<TRole, TUser>)));

            //GeevUserStore
            services.TryAddScoped<GeevUserStore<TRole, TUser>>();
            services.TryAddScoped(typeof(IUserStore<TUser>), provider => provider.GetService(typeof(GeevUserStore<TRole, TUser>)));

            //GeevRoleStore
            services.TryAddScoped<GeevRoleStore<TRole, TUser>>();
            services.TryAddScoped(typeof(IRoleStore<TRole>), provider => provider.GetService(typeof(GeevRoleStore<TRole, TUser>)));

            //GeevFeatureValueStore
            services.TryAddScoped<GeevFeatureValueStore<TTenant, TUser>>();
            services.TryAddScoped(typeof(IFeatureValueStore), provider => provider.GetService(typeof(GeevFeatureValueStore<TTenant, TUser>)));

            return new GeevIdentityBuilder(services.AddIdentity<TUser, TRole>(setupAction), typeof(TTenant));
        }
    }
}
