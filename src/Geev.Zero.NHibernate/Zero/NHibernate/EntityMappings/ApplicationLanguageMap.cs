using Geev.Localization;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class ApplicationLanguageMap : EntityMap<ApplicationLanguage>
    {
        public ApplicationLanguageMap()
            : base("GeevLanguages")
        {
            Map(x => x.TenantId);
            Map(x => x.Name);
            Map(x => x.DisplayName);
            Map(x => x.Icon);
            
            this.MapFullAudited();
        }
    }
}