using Geev.Authorization.Users;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class UserRoleMap : EntityMap<UserRole, long>
    {
        public UserRoleMap()
            : base("GeevUserRoles")
        {
            Map(x => x.TenantId);
            Map(x => x.UserId);
            Map(x => x.RoleId);
            
            this.MapCreationAudited();
        }
    }
}
