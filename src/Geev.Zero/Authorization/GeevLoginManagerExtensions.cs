using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.MultiTenancy;
using Geev.Threading;

namespace Geev.Authorization
{
    public static class GeevLogInManagerExtensions
    {
        public static GeevLoginResult<TTenant, TUser> Login<TTenant, TRole, TUser>(
            this GeevLogInManager<TTenant, TRole, TUser> logInManager, 
            string userNameOrEmailAddress, 
            string plainPassword, 
            string tenancyName = null, 
            bool shouldLockout = true)
                where TTenant : GeevTenant<TUser>
                where TRole : GeevRole<TUser>, new()
                where TUser : GeevUser<TUser>
        {
            return AsyncHelper.RunSync(
                () => logInManager.LoginAsync(
                    userNameOrEmailAddress,
                    plainPassword,
                    tenancyName,
                    shouldLockout
                )
            );
        }
    }
}
