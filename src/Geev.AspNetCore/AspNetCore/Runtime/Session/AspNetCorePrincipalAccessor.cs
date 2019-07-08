using System.Security.Claims;
using Geev.Runtime.Session;
using Microsoft.AspNetCore.Http;

namespace Geev.AspNetCore.Runtime.Session
{
    public class AspNetCorePrincipalAccessor : DefaultPrincipalAccessor
    {
        public override ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User ?? base.Principal;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetCorePrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
