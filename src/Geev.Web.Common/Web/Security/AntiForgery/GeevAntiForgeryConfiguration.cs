namespace Geev.Web.Security.AntiForgery
{
    public class GeevAntiForgeryConfiguration : IGeevAntiForgeryConfiguration
    {
        public string TokenCookieName { get; set; }

        public string TokenHeaderName { get; set; }

        public string AuthorizationCookieName { get; set; }

        public string AuthorizationCookieApplicationScheme { get; set; }
        
        public GeevAntiForgeryConfiguration()
        {
            TokenCookieName = "XSRF-TOKEN";
            TokenHeaderName = "X-XSRF-TOKEN";
            AuthorizationCookieName = ".AspNet.ApplicationCookie";
            AuthorizationCookieApplicationScheme = "Identity.Application";
        }
    }
}