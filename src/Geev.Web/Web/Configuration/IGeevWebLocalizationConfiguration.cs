namespace Geev.Web.Configuration
{
    public interface IGeevWebLocalizationConfiguration
    {
        /// <summary>
        /// Default: "Geev.Localization.CultureName".
        /// </summary>
        string CookieName { get; set; }
    }
}