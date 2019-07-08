using Geev.Authorization;
using Geev.Zero.SampleApp.Roles;
using Geev.Zero.SampleApp.Users;

namespace Geev.Zero.SampleApp.Authorization
{
    public class AppPermissionChecker : PermissionChecker<Role, User>
    {
        public AppPermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
