using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Geev.Application.Editions;
using Geev.Application.Features;
using Geev.Authorization;
using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.Configuration;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.Linq;
using Geev.MultiTenancy;
using Geev.Organizations;
using Geev.Runtime.Caching;
using Geev.Zero.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Geev.ZeroCore.SampleApp.Core
{
    public class UserManager : GeevUserManager<Role, User>
    {
        public UserManager(
            RoleManager roleManager,
            UserStore userStore,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager> logger,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings,
            ISettingManager settingManager) : base(
            roleManager,
            userStore,
            optionsAccessor,
            passwordHasher,
            userValidators,
            passwordValidators,
            keyNormalizer,
            errors,
            services,
            logger,
            permissionManager,
            unitOfWorkManager,
            cacheManager,
            organizationUnitRepository,
            userOrganizationUnitRepository,
            organizationUnitSettings,
            settingManager)
        {
        }
    }

    public class TenantManager : GeevTenantManager<Tenant, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository,
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
            EditionManager editionManager,
            IGeevZeroFeatureValueStore featureValueStore) :
            base(
                tenantRepository,
                tenantFeatureRepository,
                editionManager,
                featureValueStore)
        {
        }
    }

    public class EditionManager : GeevEditionManager
    {
        public const string DefaultEditionName = "Standard";

        public EditionManager(
            IRepository<Edition> editionRepository,
            IGeevZeroFeatureValueStore featureValueStore)
            : base(
               editionRepository,
               featureValueStore)
        {
        }
    }

    public class RoleManager : GeevRoleManager<Role, User>
    {
        public RoleManager(
            RoleStore store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager> logger,
            IPermissionManager permissionManager,
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager,
            IRoleManagementConfig roleManagementConfig,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository
        ) : base(
            store,
            roleValidators,
            keyNormalizer,
            errors,
            logger,
            permissionManager,
            cacheManager,
            unitOfWorkManager,
            roleManagementConfig,
            organizationUnitRepository,
            organizationUnitRoleRepository)
        {
        }
    }

    public class LogInManager : GeevLogInManager<Tenant, Role, User>
    {
        public LogInManager(
            GeevUserManager<Role, User> userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<Tenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            IPasswordHasher<User> passwordHasher,
            GeevRoleManager<Role, User> roleManager,
            UserClaimsPrincipalFactory claimsPrincipalFactory
        ) : base(
            userManager,
            multiTenancyConfig,
            tenantRepository,
            unitOfWorkManager,
            settingManager,
            userLoginAttemptRepository,
            userManagementConfig,
            iocResolver,
            passwordHasher,
            roleManager,
            claimsPrincipalFactory)
        {
        }
    }

    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }

    public class FeatureValueStore : GeevFeatureValueStore<Tenant, User>
    {
        public FeatureValueStore(ICacheManager cacheManager,
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
            IRepository<Tenant> tenantRepository,
            IRepository<EditionFeatureSetting, long> editionFeatureRepository,
            IFeatureManager featureManager,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                cacheManager,
                tenantFeatureRepository,
                tenantRepository,
                editionFeatureRepository,
                featureManager,
                unitOfWorkManager)
        {

        }
    }

    public class RoleStore : GeevRoleStore<Role, User>
    {
        public RoleStore(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<Role> roleRepository,
            IRepository<RolePermissionSetting, long> rolePermissionSettingRepository
        ) : base(
            unitOfWorkManager,
            roleRepository,
            rolePermissionSettingRepository)
        {
        }
    }

    public class SecurityStampValidator : GeevSecurityStampValidator<Tenant, Role, User>
    {
        public SecurityStampValidator(
            IOptions<SecurityStampValidatorOptions> options,
            SignInManager signInManager,
            ISystemClock systemClock)
            : base(options, signInManager, systemClock)
        {
        }
    }

    public class SignInManager : GeevSignInManager<Tenant, Role, User>
    {
        public SignInManager(
            UserManager userManager,
            IHttpContextAccessor contextAccessor,
            UserClaimsPrincipalFactory claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<User>> logger,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IAuthenticationSchemeProvider schemes
        ) : base(
            userManager,
            contextAccessor,
            claimsFactory,
            optionsAccessor,
            logger,
            unitOfWorkManager,
            settingManager,
            schemes)
        {
        }
    }

    public class UserStore : GeevUserStore<Role, User>
    {
        public UserStore(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<User, long> userRepository,
            IRepository<Role> roleRepository,
            IAsyncQueryableExecuter asyncQueryableExecuter,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserClaim, long> userClaimRepository,
            IRepository<UserPermissionSetting, long> userPermissionSettingRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository
            ) : base(
            unitOfWorkManager,
            userRepository,
            roleRepository,
            asyncQueryableExecuter,
            userRoleRepository,
            userLoginRepository,
            userClaimRepository,
            userPermissionSettingRepository,
            userOrganizationUnitRepository,
            organizationUnitRoleRepository)
        {
        }
    }

    public class UserClaimsPrincipalFactory : GeevUserClaimsPrincipalFactory<User, Role>
    {
        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(
                userManager,
                roleManager,
                optionsAccessor)
        {
        }

        [UnitOfWork]
        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            return await base.CreateAsync(user);
        }
    }
}
