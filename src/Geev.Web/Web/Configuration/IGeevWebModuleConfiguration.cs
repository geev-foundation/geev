using Geev.Web.Security.AntiForgery;

namespace Geev.Web.Configuration
{
    public interface IGeevWebModuleConfiguration
    {
        IGeevAntiForgeryWebConfiguration AntiForgery { get; }

        IGeevWebLocalizationConfiguration Localization { get; }
    }
}