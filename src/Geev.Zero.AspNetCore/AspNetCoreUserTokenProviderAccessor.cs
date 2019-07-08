using Geev.Authorization.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.DataProtection;

namespace Geev.Zero.AspNetCore
{
    public class AspNetCoreUserTokenProviderAccessor : IUserTokenProviderAccessor
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public AspNetCoreUserTokenProviderAccessor(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }

        public IUserTokenProvider<TUser, long> GetUserTokenProviderOrNull<TUser>()
            where TUser : GeevUser<TUser>
        {
            return new DataProtectorUserTokenProvider<TUser>(_dataProtectionProvider.CreateProtector("ASP.NET Identity"));
        }
    }
}