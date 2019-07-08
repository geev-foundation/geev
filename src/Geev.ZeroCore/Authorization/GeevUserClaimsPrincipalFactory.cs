using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.Dependency;
using Geev.Domain.Uow;
using Geev.Runtime.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Geev.Authorization
{
    public class GeevUserClaimsPrincipalFactory<TUser, TRole> : UserClaimsPrincipalFactory<TUser, TRole>, ITransientDependency
        where TRole : GeevRole<TUser>, new()
        where TUser : GeevUser<TUser>
    {
        public GeevUserClaimsPrincipalFactory(
            GeevUserManager<TRole, TUser> userManager,
            GeevRoleManager<TRole, TUser> roleManager,
            IOptions<IdentityOptions> optionsAccessor
            ) : base(userManager, roleManager, optionsAccessor)
        {

        }

        [UnitOfWork]
        public override async Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            var principal = await base.CreateAsync(user);

            if (user.TenantId.HasValue)
            {
                principal.Identities.First().AddClaim(new Claim(GeevClaimTypes.TenantId,user.TenantId.ToString()));
            }

            return principal;
        }
    }
}