using Geev.Authorization;
using Geev.Localization;

namespace Geev.Web.Common.Tests
{
    public class GeevWebCommonTestModuleAuthProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission("Permission1", new FixedLocalizableString("Permission1"));
        }
    }
}