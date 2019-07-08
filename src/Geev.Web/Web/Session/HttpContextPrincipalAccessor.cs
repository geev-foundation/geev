using System.Security.Claims;
using System.Web;
using Geev.Runtime.Session;

namespace Geev.Web.Session
{
    public class HttpContextPrincipalAccessor : DefaultPrincipalAccessor
    {
        public override ClaimsPrincipal Principal => HttpContext.Current?.User as ClaimsPrincipal ?? base.Principal;
    }
}
