using Geev.NHibernate.EntityMappings;
using Geev.Organizations;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class OrganizationUnitRoleMap : EntityMap<OrganizationUnitRole, long>
    {
        public OrganizationUnitRoleMap()
            : base("GeevOrganizationUnitRoles")
        {
            Map(x => x.TenantId);
            Map(x => x.RoleId);
            Map(x => x.OrganizationUnitId);

            this.MapCreationAudited();
        }
    }
}