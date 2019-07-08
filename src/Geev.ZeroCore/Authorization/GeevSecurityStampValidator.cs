using System.Threading.Tasks;
using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.Domain.Uow;
using Geev.MultiTenancy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Geev.Authorization
{
    public class GeevSecurityStampValidator<TTenant, TRole, TUser> : SecurityStampValidator<TUser>
        where TTenant : GeevTenant<TUser>
        where TRole : GeevRole<TUser>, new()
        where TUser : GeevUser<TUser>
    {
        public GeevSecurityStampValidator(
            IOptions<SecurityStampValidatorOptions> options,
            GeevSignInManager<TTenant, TRole, TUser> signInManager,
            ISystemClock systemClock)
            : base(
                options, 
                signInManager,
                systemClock)
        {
        }

        [UnitOfWork]
        public override Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            return base.ValidateAsync(context);
        }
    }
}
