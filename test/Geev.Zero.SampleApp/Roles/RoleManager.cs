using Geev.Authorization;
using Geev.Authorization.Roles;
using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.Organizations;
using Geev.Runtime.Caching;
using Geev.Zero.Configuration;
using Geev.Zero.SampleApp.Users;

namespace Geev.Zero.SampleApp.Roles
{
    public class RoleManager : GeevRoleManager<Role, User>
    {
        public RoleManager(
            RoleStore store,
            IPermissionManager permissionManager,
            IRoleManagementConfig roleManagementConfig,
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository)
            : base(
            store,
            permissionManager,
            roleManagementConfig,
            cacheManager,
            unitOfWorkManager,
                organizationUnitRepository,
                organizationUnitRoleRepository)
        {
        }
    }
}