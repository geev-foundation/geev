using Geev.Authorization;
using Geev.Authorization.Users;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class UserLoginAttemptMap : EntityMap<UserLoginAttempt, long>
    {
        public UserLoginAttemptMap() 
            : base("GeevUserLoginAttempts")
        {
            Map(x => x.TenantId);
            Map(x => x.TenancyName);
            Map(x => x.UserId);
            Map(x => x.UserNameOrEmailAddress);
            Map(x => x.ClientIpAddress);
            Map(x => x.ClientName);
            Map(x => x.BrowserInfo);
            Map(x => x.Result).CustomType<GeevLoginResultType>();

            this.MapCreationTime();
        }
    }
}