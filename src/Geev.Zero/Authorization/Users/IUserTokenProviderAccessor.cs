using Microsoft.AspNet.Identity;

namespace Geev.Authorization.Users
{
    public interface IUserTokenProviderAccessor
    {
        IUserTokenProvider<TUser, long> GetUserTokenProviderOrNull<TUser>() 
            where TUser : GeevUser<TUser>;
    }
}