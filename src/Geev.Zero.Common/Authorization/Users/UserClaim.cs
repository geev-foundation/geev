using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Geev.Domain.Entities;
using Geev.Domain.Entities.Auditing;

namespace Geev.Authorization.Users
{
    [Table("UserClaims")]
    public class UserClaim : CreationAuditedEntity<long>, IMayHaveTenant
    {
        /// <summary>
        /// Maximum length of the <see cref="ClaimType"/> property.
        /// </summary>
        public const int MaxClaimTypeLength = 256;

        public virtual int? TenantId { get; set; }

        public virtual long UserId { get; set; }

        [StringLength(MaxClaimTypeLength)]
        public virtual string ClaimType { get; set; }

        public virtual string ClaimValue { get; set; }

        public UserClaim()
        {
            
        }

        public UserClaim(GeevUserBase user, Claim claim)
        {
            TenantId = user.TenantId;
            UserId = user.Id;
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
    }
}
