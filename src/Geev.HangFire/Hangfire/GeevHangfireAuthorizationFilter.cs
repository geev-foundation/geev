using Geev.Authorization;
using Geev.Dependency;
using Geev.Extensions;
using Geev.Runtime.Session;
using Hangfire.Dashboard;

namespace Geev.Hangfire
{
    public class GeevHangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public IIocResolver IocResolver { get; set; }

        private readonly string _requiredPermissionName;

        public GeevHangfireAuthorizationFilter(string requiredPermissionName = null)
        {
            _requiredPermissionName = requiredPermissionName;

            IocResolver = IocManager.Instance;
        }

        public bool Authorize(DashboardContext context)
        {
            if (!IsLoggedIn())
            {
                return false;
            }

            if (!_requiredPermissionName.IsNullOrEmpty() && !IsPermissionGranted(_requiredPermissionName))
            {
                return false;
            }

            return true;
        }

        private bool IsLoggedIn()
        {
            using (var geevSession = IocResolver.ResolveAsDisposable<IGeevSession>())
            {
                return geevSession.Object.UserId.HasValue;
            }
        }

        private bool IsPermissionGranted(string requiredPermissionName)
        {
            using (var permissionChecker = IocResolver.ResolveAsDisposable<IPermissionChecker>())
            {
                return permissionChecker.Object.IsGranted(requiredPermissionName);
            }
        }
    }
}
