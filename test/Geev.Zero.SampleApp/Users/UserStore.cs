using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.Organizations;
using Geev.Zero.SampleApp.Roles;

namespace Geev.Zero.SampleApp.Users
{
    public class UserStore : GeevUserStore<Role, User>
    {
        public UserStore(
            IRepository<User, long> userRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IRepository<UserPermissionSetting, long> userPermissionSettingRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<UserClaim, long> userClaimRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository)
            : base(
                userRepository,
                userLoginRepository,
                userRoleRepository,
                roleRepository,
                userPermissionSettingRepository,
                unitOfWorkManager,
                userClaimRepository,
                userOrganizationUnitRepository,
                organizationUnitRoleRepository)
        {

        }
    }
}