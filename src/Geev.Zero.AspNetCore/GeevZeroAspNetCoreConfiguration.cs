using Microsoft.AspNet.Identity;

namespace Geev.Zero.AspNetCore
{
    internal class GeevZeroAspNetCoreConfiguration : IGeevZeroAspNetCoreConfiguration
    {
        public string AuthenticationScheme { get; set; }

        public string TwoFactorAuthenticationScheme { get; set; }

        public string TwoFactorRememberBrowserAuthenticationScheme { get; set; }

        public GeevZeroAspNetCoreConfiguration()
        {
            AuthenticationScheme = "AppAuthenticationScheme";
            TwoFactorAuthenticationScheme = DefaultAuthenticationTypes.TwoFactorCookie;
            TwoFactorRememberBrowserAuthenticationScheme = DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie;
        }
    }
}
