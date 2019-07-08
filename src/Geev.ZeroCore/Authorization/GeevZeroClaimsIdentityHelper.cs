using System;
using System.Security.Claims;
using Geev.Runtime.Security;

namespace Geev.Authorization
{
    internal static class GeevZeroClaimsIdentityHelper
    {
        public static int? GetTenantId(ClaimsPrincipal principal)
        {
            var tenantIdOrNull = principal?.FindFirstValue(GeevClaimTypes.TenantId);
            if (tenantIdOrNull == null)
            {
                return null;
            }

            return Convert.ToInt32(tenantIdOrNull);
        }
    }
}