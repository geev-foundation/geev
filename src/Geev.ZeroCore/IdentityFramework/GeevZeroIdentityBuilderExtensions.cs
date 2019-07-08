using Geev.Application.Editions;
using Geev.Application.Features;
using Geev.Authorization;
using Microsoft.AspNetCore.Identity;
using Geev.Authorization.Users;
using Geev.Authorization.Roles;
using Geev.MultiTenancy;

// ReSharper disable once CheckNamespace - This is done to add extension methods to Microsoft.Extensions.DependencyInjection namespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class GeevZeroIdentityBuilderExtensions
    {
        public static GeevIdentityBuilder AddGeevTenantManager<TTenantManager>(this GeevIdentityBuilder builder)
            where TTenantManager : class
        {
            var type = typeof(TTenantManager);
            var geevManagerType = typeof(GeevTenantManager<,>).MakeGenericType(builder.TenantType, builder.UserType);
            builder.Services.AddScoped(type, provider => provider.GetRequiredService(geevManagerType));
            builder.Services.AddScoped(geevManagerType, type);
            return builder;
        }

        public static GeevIdentityBuilder AddGeevEditionManager<TEditionManager>(this GeevIdentityBuilder builder)
            where TEditionManager : class
        {
            var type = typeof(TEditionManager);
            var geevManagerType = typeof(GeevEditionManager);
            builder.Services.AddScoped(type, provider => provider.GetRequiredService(geevManagerType));
            builder.Services.AddScoped(geevManagerType, type);
            return builder;
        }

        public static GeevIdentityBuilder AddGeevRoleManager<TRoleManager>(this GeevIdentityBuilder builder)
            where TRoleManager : class
        {
            var geevManagerType = typeof(GeevRoleManager<,>).MakeGenericType(builder.RoleType, builder.UserType);
            var managerType = typeof(RoleManager<>).MakeGenericType(builder.RoleType);
            builder.Services.AddScoped(geevManagerType, services => services.GetRequiredService(managerType));
            builder.AddRoleManager<TRoleManager>();
            return builder;
        }

        public static GeevIdentityBuilder AddGeevUserManager<TUserManager>(this GeevIdentityBuilder builder)
            where TUserManager : class
        {
            var geevManagerType = typeof(GeevUserManager<,>).MakeGenericType(builder.RoleType, builder.UserType);
            var managerType = typeof(UserManager<>).MakeGenericType(builder.UserType);
            builder.Services.AddScoped(geevManagerType, services => services.GetRequiredService(managerType));
            builder.AddUserManager<TUserManager>();
            return builder;
        }

        public static GeevIdentityBuilder AddGeevSignInManager<TSignInManager>(this GeevIdentityBuilder builder)
            where TSignInManager : class
        {
            var geevManagerType = typeof(GeevSignInManager<,,>).MakeGenericType(builder.TenantType, builder.RoleType, builder.UserType);
            var managerType = typeof(SignInManager<>).MakeGenericType(builder.UserType);
            builder.Services.AddScoped(geevManagerType, services => services.GetRequiredService(managerType));
            builder.AddSignInManager<TSignInManager>();
            return builder;
        }

        public static GeevIdentityBuilder AddGeevLogInManager<TLogInManager>(this GeevIdentityBuilder builder)
            where TLogInManager : class
        {
            var type = typeof(TLogInManager);
            var geevManagerType = typeof(GeevLogInManager<,,>).MakeGenericType(builder.TenantType, builder.RoleType, builder.UserType);
            builder.Services.AddScoped(type, provider => provider.GetService(geevManagerType));
            builder.Services.AddScoped(geevManagerType, type);
            return builder;
        }

        public static GeevIdentityBuilder AddGeevUserClaimsPrincipalFactory<TUserClaimsPrincipalFactory>(this GeevIdentityBuilder builder)
            where TUserClaimsPrincipalFactory : class
        {
            var type = typeof(TUserClaimsPrincipalFactory);
            builder.Services.AddScoped(typeof(UserClaimsPrincipalFactory<,>).MakeGenericType(builder.UserType, builder.RoleType), services => services.GetRequiredService(type));
            builder.Services.AddScoped(typeof(GeevUserClaimsPrincipalFactory<,>).MakeGenericType(builder.UserType, builder.RoleType), services => services.GetRequiredService(type));
            builder.Services.AddScoped(typeof(IUserClaimsPrincipalFactory<>).MakeGenericType(builder.UserType), services => services.GetRequiredService(type));
            builder.Services.AddScoped(type);
            return builder;
        }

        public static GeevIdentityBuilder AddGeevSecurityStampValidator<TSecurityStampValidator>(this GeevIdentityBuilder builder)
            where TSecurityStampValidator : class, ISecurityStampValidator
        {
            var type = typeof(TSecurityStampValidator);
            builder.Services.AddScoped(typeof(SecurityStampValidator<>).MakeGenericType(builder.UserType), services => services.GetRequiredService(type));
            builder.Services.AddScoped(typeof(GeevSecurityStampValidator<,,>).MakeGenericType(builder.TenantType, builder.RoleType, builder.UserType), services => services.GetRequiredService(type));
            builder.Services.AddScoped(typeof(ISecurityStampValidator), services => services.GetRequiredService(type));
            builder.Services.AddScoped(type);
            return builder;
        }

        public static GeevIdentityBuilder AddPermissionChecker<TPermissionChecker>(this GeevIdentityBuilder builder)
            where TPermissionChecker : class
        {
            var type = typeof(TPermissionChecker);
            var checkerType = typeof(PermissionChecker<,>).MakeGenericType(builder.RoleType, builder.UserType);
            builder.Services.AddScoped(type);
            builder.Services.AddScoped(checkerType, provider => provider.GetService(type));
            builder.Services.AddScoped(typeof(IPermissionChecker), provider => provider.GetService(type));
            return builder;
        }

        public static GeevIdentityBuilder AddGeevUserStore<TUserStore>(this GeevIdentityBuilder builder)
            where TUserStore : class
        {
            var type = typeof(TUserStore);
            var geevStoreType = typeof(GeevUserStore<,>).MakeGenericType(builder.RoleType, builder.UserType);
            var storeType = typeof(IUserStore<>).MakeGenericType(builder.UserType);
            builder.Services.AddScoped(type);
            builder.Services.AddScoped(geevStoreType, services => services.GetRequiredService(type));
            builder.Services.AddScoped(storeType, services => services.GetRequiredService(type));
            return builder;
        }

        public static GeevIdentityBuilder AddGeevRoleStore<TRoleStore>(this GeevIdentityBuilder builder)
            where TRoleStore : class
        {
            var type = typeof(TRoleStore);
            var geevStoreType = typeof(GeevRoleStore<,>).MakeGenericType(builder.RoleType, builder.UserType);
            var storeType = typeof(IRoleStore<>).MakeGenericType(builder.RoleType);
            builder.Services.AddScoped(type);
            builder.Services.AddScoped(geevStoreType, services => services.GetRequiredService(type));
            builder.Services.AddScoped(storeType, services => services.GetRequiredService(type));
            return builder;
        }

        public static GeevIdentityBuilder AddFeatureValueStore<TFeatureValueStore>(this GeevIdentityBuilder builder)
            where TFeatureValueStore : class
        {
            var type = typeof(TFeatureValueStore);
            var storeType = typeof(GeevFeatureValueStore<,>).MakeGenericType(builder.TenantType, builder.UserType);
            builder.Services.AddScoped(type);
            builder.Services.AddScoped(storeType, provider => provider.GetService(type));
            builder.Services.AddScoped(typeof(IFeatureValueStore), provider => provider.GetService(type));
            return builder;
        }
    }
}
