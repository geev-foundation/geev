using System.Security.Claims;
using Geev.MultiTenancy;

namespace Geev.Authorization.Users
{
    public class GeevLoginResult<TTenant, TUser>
        where TTenant : GeevTenant<TUser>
        where TUser : GeevUserBase
    {
        public GeevLoginResultType Result { get; private set; }

        public TTenant Tenant { get; private set; }

        public TUser User { get; private set; }

        public ClaimsIdentity Identity { get; private set; }

        public GeevLoginResult(GeevLoginResultType result, TTenant tenant = null, TUser user = null)
        {
            Result = result;
            Tenant = tenant;
            User = user;
        }

        public GeevLoginResult(TTenant tenant, TUser user, ClaimsIdentity identity)
            : this(GeevLoginResultType.Success, tenant)
        {
            User = user;
            Identity = identity;
        }
    }
}