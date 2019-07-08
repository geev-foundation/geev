namespace Geev.Web.Models.GeevUserConfiguration
{
    public class GeevUserAntiForgeryConfigDto
    {
        public string TokenCookieName { get; set; }

        public string TokenHeaderName { get; set; }
    }
}