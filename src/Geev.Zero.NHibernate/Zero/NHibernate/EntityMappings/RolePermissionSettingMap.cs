using Geev.Authorization.Roles;
using FluentNHibernate.Mapping;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class RolePermissionSettingMap : SubclassMap<RolePermissionSetting>
    {
        public RolePermissionSettingMap()
        {
            DiscriminatorValue("RolePermissionSetting");

            Map(x => x.RoleId).Not.Nullable();
        }
    }
}