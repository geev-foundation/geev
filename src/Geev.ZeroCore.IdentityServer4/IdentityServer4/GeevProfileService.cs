using System.Threading.Tasks;
using Geev.Authorization.Users;
using Geev.Domain.Uow;
using Geev.Runtime.Security;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;

namespace Geev.IdentityServer4
{
    public class GeevProfileService<TUser> : ProfileService<TUser>
        where TUser : GeevUser<TUser>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly UserManager<TUser> _userManager;

        public GeevProfileService(
            UserManager<TUser> userManager,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IUnitOfWorkManager unitOfWorkManager
        ) : base(userManager, claimsFactory)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _userManager = userManager;
        }

        [UnitOfWork]
        public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var tenantId = context.Subject.Identity.GetTenantId();
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                await base.GetProfileDataAsync(context);
            }
        }

        [UnitOfWork]
        public override async Task IsActiveAsync(IsActiveContext context)
        {
            var tenantId = context.Subject.Identity.GetTenantId();
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                await base.IsActiveAsync(context);

                if (!context.IsActive)
                {
                    return;
                }

                var sub = context.Subject.GetSubjectId();
                var user = await _userManager.FindByIdAsync(sub);

                context.IsActive = user != null && user.IsActive;
            }
        }
    }
}
