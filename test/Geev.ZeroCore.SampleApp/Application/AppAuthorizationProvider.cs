using Geev.Authorization;
using Geev.Localization;
using Geev.MultiTenancy;

namespace Geev.ZeroCore.SampleApp.Application
{
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(AppPermissions.TestPermission, AppLocalizationHelper.L("TestPermission"), multiTenancySides: MultiTenancySides.Tenant);

            context.CreatePermission("Permission1", new FixedLocalizableString("Permission1"));
            context.CreatePermission("Permission2", new FixedLocalizableString("Permission2"));
        }
    }
}
