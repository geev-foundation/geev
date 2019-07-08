using Geev.Authorization;
using Geev.Authorization.Users;
using Geev.Configuration;
using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.IdentityFramework;
using Geev.Localization;
using Geev.Organizations;
using Geev.Runtime.Caching;
using Geev.Zero.SampleApp.Roles;

namespace Geev.Zero.SampleApp.Users
{
    public class UserManager : GeevUserManager<Role, User>
    {
        public UserManager(
            UserStore userStore, 
            RoleManager roleManager, 
            IPermissionManager permissionManager, 
            IUnitOfWorkManager unitOfWorkManager, 
            ICacheManager cacheManager, 
            IRepository<OrganizationUnit, long> organizationUnitRepository, 
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository, 
            IOrganizationUnitSettings organizationUnitSettings,
            ILocalizationManager localizationManager,
            ISettingManager settingManager,
            IdentityEmailMessageService emailService,
            IUserTokenProviderAccessor userTokenProviderAccessor)
            : base(
                  userStore, 
                  roleManager, 
                  permissionManager, 
                  unitOfWorkManager, 
                  cacheManager, 
                  organizationUnitRepository, 
                  userOrganizationUnitRepository, 
                  organizationUnitSettings,
                  localizationManager,
                  emailService,
                  settingManager,
                  userTokenProviderAccessor)
        {
        }
    }
}