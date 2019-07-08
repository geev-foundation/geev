using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.Domain.Repositories;
using Geev.Zero.SampleApp.Users;

namespace Geev.Zero.SampleApp.Roles
{
    public class RoleStore : GeevRoleStore<Role, User>
    {
        public RoleStore(
            IRepository<Role> roleRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<RolePermissionSetting, long> rolePermissionSettingRepository)
            : base(
                roleRepository,
                userRoleRepository,
                rolePermissionSettingRepository)
        {
        }
    }
}