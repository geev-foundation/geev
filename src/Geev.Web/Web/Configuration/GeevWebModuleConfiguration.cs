using Geev.Web.Security.AntiForgery;

namespace Geev.Web.Configuration
{
    public class GeevWebModuleConfiguration : IGeevWebModuleConfiguration
    {
        public IGeevAntiForgeryWebConfiguration AntiForgery { get; }
        public IGeevWebLocalizationConfiguration Localization { get; }

        public GeevWebModuleConfiguration(
            IGeevAntiForgeryWebConfiguration antiForgery, 
            IGeevWebLocalizationConfiguration localization)
        {
            AntiForgery = antiForgery;
            Localization = localization;
        }
    }
}