using System.IdentityModel.Tokens.Jwt;
using Geev.Runtime.Security;

namespace Geev.IdentityServer4
{
    public class GeevIdentityServerOptions
    {
        /// <summary>
        /// Updates <see cref="JwtSecurityTokenHandler.DefaultInboundClaimTypeMap"/> to be compatible with identity server claims.
        /// Default: true.
        /// </summary>
        public bool UpdateJwtSecurityTokenHandlerDefaultInboundClaimTypeMap { get; set; } = true;

        /// <summary>
        /// Updates <see cref="GeevClaimTypes"/> to be compatible with identity server claims.
        /// Default: true.
        /// </summary>
        public bool UpdateGeevClaimTypes { get; set; } = true;
    }
}