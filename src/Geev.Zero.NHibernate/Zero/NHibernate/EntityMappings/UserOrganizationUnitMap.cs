using Geev.Authorization.Users;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class UserOrganizationUnitMap : EntityMap<UserOrganizationUnit, long>
    {
        public UserOrganizationUnitMap()
            : base("GeevUserOrganizationUnits")
        {
            Map(x => x.TenantId);
            Map(x => x.UserId);
            Map(x => x.OrganizationUnitId);

            this.MapCreationAudited();
        }
    }
}