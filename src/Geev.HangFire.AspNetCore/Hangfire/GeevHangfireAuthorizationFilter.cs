using Geev.Authorization;
using Geev.Extensions;
using Geev.Runtime.Session;
using Hangfire.Dashboard;
using Microsoft.Extensions.DependencyInjection;

namespace Geev.Hangfire
{
    public class GeevHangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string _requiredPermissionName;

        public GeevHangfireAuthorizationFilter(string requiredPermissionName = null)
        {
            _requiredPermissionName = requiredPermissionName;
        }

        public bool Authorize(DashboardContext context)
        {
            if (!IsLoggedIn(context))
            {
                return false;
            }

            if (!_requiredPermissionName.IsNullOrEmpty() && !IsPermissionGranted(context, _requiredPermissionName))
            {
                return false;
            }

            return true;
        }

        private static bool IsLoggedIn(DashboardContext context)
        {
            var geevSession = context.GetHttpContext().RequestServices.GetRequiredService<IGeevSession>();
            return geevSession.UserId.HasValue;
        }

        private static bool IsPermissionGranted(DashboardContext context, string requiredPermissionName)
        {
            var permissionChecker = context.GetHttpContext().RequestServices.GetRequiredService<IPermissionChecker>();
            return permissionChecker.IsGranted(requiredPermissionName);
        }
    }
}
