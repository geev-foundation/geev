using Geev.Authorization;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class PermissionSettingMap : EntityMap<PermissionSetting, long>
    {
        public PermissionSettingMap()
            : base("GeevPermissions")
        {
            DiscriminateSubClassesOnColumn("Discriminator");

            Map(x => x.Name);
            Map(x => x.IsGranted);

            this.MapCreationAudited();
        }
    }
}