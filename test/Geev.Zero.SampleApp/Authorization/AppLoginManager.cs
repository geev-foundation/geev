using Geev.Authorization;
using Geev.Authorization.Users;
using Geev.Configuration;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.Zero.Configuration;
using Geev.Zero.SampleApp.MultiTenancy;
using Geev.Zero.SampleApp.Roles;
using Geev.Zero.SampleApp.Users;

namespace Geev.Zero.SampleApp.Authorization
{
    public class AppLogInManager : GeevLogInManager<Tenant, Role, User>
    {
        public AppLogInManager(
            UserManager userManager, 
            IMultiTenancyConfig multiTenancyConfig, 
            IRepository<Tenant> tenantRepository, 
            IUnitOfWorkManager unitOfWorkManager, 
            ISettingManager settingManager, 
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository, 
            IUserManagementConfig userManagementConfig, IIocResolver iocResolver, 
            RoleManager roleManager) 
            : base(
                  userManager,
                  multiTenancyConfig, 
                  tenantRepository, 
                  unitOfWorkManager, 
                  settingManager, 
                  userLoginAttemptRepository, 
                  userManagementConfig, 
                  iocResolver, 
                  roleManager)
        {
        }
    }
}
