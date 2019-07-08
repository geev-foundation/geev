using Geev.Authorization.Users;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class UserLoginMap : EntityMap<UserLogin, long>
    {
        public UserLoginMap()
            : base("GeevUserLogins")
        {
            Map(x => x.UserId);
            Map(x => x.LoginProvider);
            Map(x => x.ProviderKey);
        }
    }
}