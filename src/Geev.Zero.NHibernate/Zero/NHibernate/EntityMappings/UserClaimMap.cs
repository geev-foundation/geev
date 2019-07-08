using Geev.Authorization.Users;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class UserClaimMap : EntityMap<UserClaim, long>
    {
        public UserClaimMap()
            : base("GeevUserClaims")
        {
            Map(x => x.TenantId);
            Map(x => x.UserId);
            Map(x => x.ClaimType);
            Map(x => x.ClaimValue);

            this.MapCreationAudited();
        }
    }
}