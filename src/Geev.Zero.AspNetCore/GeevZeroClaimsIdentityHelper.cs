using System;
using System.Security.Claims;
using System.Security.Principal;
using Geev.Runtime.Security;
using Microsoft.AspNet.Identity;

namespace Geev.Zero.AspNetCore
{
    internal static class GeevZeroClaimsIdentityHelper
    {
        public static int? GetTenantId(IIdentity identity)
        {
            if (identity == null)
            {
                return null;
            }

            var claimsIdentity = identity as ClaimsIdentity;

            var tenantIdOrNull = claimsIdentity?.FindFirstValue(GeevClaimTypes.TenantId);
            if (tenantIdOrNull == null)
            {
                return null;
            }

            return Convert.ToInt32(tenantIdOrNull);
        }
    }
}