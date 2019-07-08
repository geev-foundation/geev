using Microsoft.AspNet.Identity;

namespace Geev.Zero.AspNetCore
{
    public interface IGeevZeroAspNetCoreConfiguration
    {
        /// <summary>
        /// Authentication scheme of the application.
        /// </summary>
        string AuthenticationScheme { get; set; }

        /// <summary>
        /// Default value: <see cref="DefaultAuthenticationTypes.TwoFactorCookie"/>.
        /// </summary>
        string TwoFactorAuthenticationScheme { get; set; }

        /// <summary>
        /// Default value: <see cref="DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie"/>.
        /// </summary>
        string TwoFactorRememberBrowserAuthenticationScheme { get; set; }
    }
}