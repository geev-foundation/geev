using Geev.Application.Features;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class FeatureSettingMap : EntityMap<FeatureSetting, long>
    {
        public FeatureSettingMap()
            : base("GeevFeatures")
        {
            DiscriminateSubClassesOnColumn("Discriminator");

            Map(x => x.Name);
            Map(x => x.Value);
            
            this.MapCreationAudited();
        }
    }
}