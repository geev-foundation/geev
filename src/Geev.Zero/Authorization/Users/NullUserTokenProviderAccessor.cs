using Geev.Dependency;
using Microsoft.AspNet.Identity;

namespace Geev.Authorization.Users
{
    public class NullUserTokenProviderAccessor : IUserTokenProviderAccessor, ISingletonDependency
    {
        public IUserTokenProvider<TUser, long> GetUserTokenProviderOrNull<TUser>() where TUser : GeevUser<TUser>
        {
            return null;
        }
    }
}