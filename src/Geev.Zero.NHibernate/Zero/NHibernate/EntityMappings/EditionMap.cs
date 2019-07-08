using Geev.Application.Editions;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class EditionMap : EntityMap<Edition>
    {
        public EditionMap()
            : base("GeevEditions")
        {
            Map(x => x.Name);
            Map(x => x.DisplayName);
            
            this.MapFullAudited();
        }
    }
}