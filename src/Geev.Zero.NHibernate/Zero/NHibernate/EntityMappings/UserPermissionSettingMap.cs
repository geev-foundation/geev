using Geev.Authorization.Users;
using FluentNHibernate.Mapping;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class UserPermissionSettingMap : SubclassMap<UserPermissionSetting>
    {
        public UserPermissionSettingMap()
        {
            DiscriminatorValue("UserPermissionSetting");

            Map(x => x.UserId).Not.Nullable();
        }
    }
}