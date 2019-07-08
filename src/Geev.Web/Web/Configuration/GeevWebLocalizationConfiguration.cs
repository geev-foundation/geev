namespace Geev.Web.Configuration
{
    public class GeevWebLocalizationConfiguration : IGeevWebLocalizationConfiguration
    {
        public string CookieName { get; set; }

        public GeevWebLocalizationConfiguration()
        {
            CookieName = "Geev.Localization.CultureName";
        }
    }
}