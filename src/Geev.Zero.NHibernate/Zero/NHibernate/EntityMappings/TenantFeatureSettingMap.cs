using Geev.MultiTenancy;
using FluentNHibernate.Mapping;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class TenantFeatureSettingMap : SubclassMap<TenantFeatureSetting>
    {
        public TenantFeatureSettingMap()
        {
            DiscriminatorValue("TenantFeatureSetting");

            Map(x => x.TenantId);
        }
    }
}