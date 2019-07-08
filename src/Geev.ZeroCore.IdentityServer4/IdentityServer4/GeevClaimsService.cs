using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Geev.Runtime.Security;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;

namespace Geev.IdentityServer4
{
    public class GeevClaimsService : DefaultClaimsService
    {
        public GeevClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger)
            : base(profile, logger)
        {
        }

        protected override IEnumerable<Claim> GetOptionalClaims(ClaimsPrincipal subject)
        {
            var tenantClaim = subject.FindFirst(GeevClaimTypes.TenantId);
            if (tenantClaim == null)
            {
                return base.GetOptionalClaims(subject);
            }
            else
            {
                return base.GetOptionalClaims(subject).Union(new[] { tenantClaim });
            }
        }
    }
}
