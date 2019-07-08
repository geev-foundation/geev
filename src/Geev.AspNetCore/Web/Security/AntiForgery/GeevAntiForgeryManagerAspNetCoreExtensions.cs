using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;

namespace Geev.Web.Security.AntiForgery
{
    public static class GeevAntiForgeryManagerAspNetCoreExtensions
    {
        public static void SetCookie(this IGeevAntiForgeryManager manager, HttpContext context, IIdentity identity = null)
        {
            if (identity != null)
            {
                context.User = new ClaimsPrincipal(identity);
            }

            context.Response.Cookies.Append(manager.Configuration.TokenCookieName, manager.GenerateToken());
        }
    }
}
