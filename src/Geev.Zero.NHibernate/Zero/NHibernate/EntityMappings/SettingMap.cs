using Geev.Configuration;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class SettingMap : EntityMap<Setting, long>
    {
        public SettingMap()
            : base("GeevSettings")
        {
            Map(x => x.TenantId);
            Map(x => x.UserId);
            Map(x => x.Name);
            Map(x => x.Value);

            this.MapAudited();
        }
    }
}