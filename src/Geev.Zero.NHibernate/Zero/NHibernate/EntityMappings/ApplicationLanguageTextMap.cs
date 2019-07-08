using Geev.Localization;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class ApplicationLanguageTextMap : EntityMap<ApplicationLanguageText, long>
    {
        public ApplicationLanguageTextMap()
            : base("GeevLanguageTexts")
        {
            Map(x => x.TenantId);
            Map(x => x.LanguageName);
            Map(x => x.Source);
            Map(x => x.Key);
            Map(x => x.Value);
            
            this.MapAudited();
        }
    }
}